using GestionTarjetaCredito.Application.Common.Exceptions;
using GestionTarjetaCredito.Application.Interfaces;
using GestionTarjetaCredito.Domain.Entities;
using GestionTarjetaCredito.Domain.Enums;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler
        : IRequestHandler<CreatePaymentCommand, int>
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(
            ICreditCardRepository creditCardRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _creditCardRepository = creditCardRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(
            CreatePaymentCommand request,
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

            if (usedBalance <= 0)
            {
                throw new BusinessException(
                    "La tarjeta no tiene saldo pendiente.");
            }

            if (request.Amount > usedBalance)
            {
                throw new BusinessException(
                    $"El monto del pago excede el saldo pendiente de {usedBalance:C2}.");
            }

            var transaction = new Transaction
            {
                CreditCardId = request.CreditCardId,
                TransactionType = TransactionType.Payment,
                TransactionDate = request.TransactionDate,
                Description = "Pago de tarjeta de crédito",
                Amount = request.Amount,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var transactionId = await _transactionRepository
                    .CreatePaymentAsync(transaction, _unitOfWork);

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