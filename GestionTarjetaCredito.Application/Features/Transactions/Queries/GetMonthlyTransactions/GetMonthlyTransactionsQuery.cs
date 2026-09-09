using GestionTarjetaCredito.Application.DTOs;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.Transactions.Queries.GetMonthlyTransactions
{
    public class GetMonthlyTransactionsQuery
        : IRequest<IEnumerable<TransactionDto>>
    {
        public int CreditCardId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }
    }
}