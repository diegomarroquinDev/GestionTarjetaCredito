using GestionTarjetaCredito.Application.Common.Exceptions;
using GestionTarjetaCredito.Application.DTOs;
using GestionTarjetaCredito.Application.Interfaces;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.Transactions.Queries.GetMonthlyTransactions
{
    public class GetMonthlyTransactionsQueryHandler
        : IRequestHandler<GetMonthlyTransactionsQuery, IEnumerable<TransactionDto>>
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ITransactionRepository _transactionRepository;

        public GetMonthlyTransactionsQueryHandler(
            ICreditCardRepository creditCardRepository,
            ITransactionRepository transactionRepository)
        {
            _creditCardRepository = creditCardRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<TransactionDto>> Handle(
            GetMonthlyTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            var creditCard = await _creditCardRepository
                .GetByIdAsync(request.CreditCardId);

            if (creditCard is null)
            {
                throw new NotFoundException(
                    $"No se encontró la tarjeta con Id {request.CreditCardId}.");
            }

            var transactions = await _transactionRepository
                .GetMonthlyTransactionsAsync(
                    request.CreditCardId,
                    request.Year,
                    request.Month);

            return transactions
                .OrderByDescending(x => x.TransactionDate)
                .ThenByDescending(x => x.Id)
                .Select(x => new TransactionDto
                {
                    Id = x.Id,
                    TransactionDate = x.TransactionDate,
                    TransactionType = x.TransactionType.ToString(),
                    Description = x.Description,
                    Amount = x.Amount
                })
                .ToList();
        }
    }
}