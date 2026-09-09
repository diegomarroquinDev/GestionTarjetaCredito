using GestionTarjetaCredito.Application.Common.Exceptions;
using GestionTarjetaCredito.Application.Interfaces;
using GestionTarjetaCredito.Domain.Entities;
using GestionTarjetaCredito.Domain.Enums;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.Purchases.Commands.CreatePurchase
{
    public class CreatePurchaseCommandHandler
        : IRequestHandler<CreatePurchaseCommand, int>
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePurchaseCommandHandler(
            ICreditCardRepository creditCardRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _creditCardRepository = creditCardRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(
            CreatePurchaseCommand request,
            CancellationToken cancellationToken)
        {
            var creditCard = await _creditCardRepository
                .GetByIdAsync(request.CreditCardId);

            if (creditCard is null)
            {
                throw new NotFoundException(
                    $"No se encontró la tarjeta con Id {request.CreditCardId}.");
            }

            if (!creditCard.IsActive)
            {
                throw new BusinessException(
                    "La tarjeta se encuentra inactiva.");
            }

            var transactions = await _transactionRepository
                .GetByCreditCardIdAsync(request.CreditCardId);

            var transactionList = transactions.ToList();

            var totalPurchases = transactionList
                .Where(x => x.TransactionType == TransactionType.Purchase)
                .Sum(x => x.Amount);

            var totalPayments = transactionList
                .Where(x => x.TransactionType == TransactionType.Payment)
                .Sum(x => x.Amount);

            var usedBalance = totalPurchases - totalPayments;

            var availableBalance = creditCard.CreditLimit - usedBalance;

            if (request.Amount > availableBalance)
            {
                throw new BusinessException(
                    $"El monto de la compra excede el crédito disponible de {availableBalance:C2}.");
            }

            var transaction = new Transaction
            {
                CreditCardId = request.CreditCardId,
                TransactionType = TransactionType.Purchase,
                TransactionDate = request.TransactionDate,
                Description = request.Description.Trim(),
                Amount = request.Amount,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var transactionId = await _transactionRepository
                    .CreatePurchaseAsync(transaction, _unitOfWork);

                await _unitOfWork.CommitAsync();

                return transactionId;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}