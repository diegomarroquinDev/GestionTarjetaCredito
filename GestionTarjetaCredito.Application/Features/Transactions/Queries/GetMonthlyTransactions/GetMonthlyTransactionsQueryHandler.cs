using AutoMapper;
using GestionTarjetaCredito.Application.Common.Exceptions;
using GestionTarjetaCredito.Application.DTOs;
using GestionTarjetaCredito.Application.Interfaces;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.Transactions.Queries.GetMonthlyTransactions
{
    public class GetMonthlyTransactionsQueryHandler
        : IRequestHandler<
            GetMonthlyTransactionsQuery,
            IEnumerable<TransactionDto>>
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetMonthlyTransactionsQueryHandler(
            ICreditCardRepository creditCardRepository,
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _creditCardRepository = creditCardRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
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

            var orderedTransactions = transactions
                .OrderByDescending(x => x.TransactionDate)
                .ThenByDescending(x => x.Id);

            return _mapper.Map<IEnumerable<TransactionDto>>(
                orderedTransactions);
        }
    }
}