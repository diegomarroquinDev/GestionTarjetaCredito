using GestionTarjetaCredito.Application.DTOs;
using GestionTarjetaCredito.Application.Interfaces;
using GestionTarjetaCredito.Domain.Constants;
using GestionTarjetaCredito.Domain.Enums;
using GestionTarjetaCredito.Application.Common.Exceptions;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.CreditCards.Queries.GetCreditCardStatement
{
    public class GetCreditCardStatementQueryHandler
        : IRequestHandler<GetCreditCardStatementQuery, CreditCardStatementDto>
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IFinancialConfigurationRepository _financialConfigurationRepository;

        public GetCreditCardStatementQueryHandler(
            ICreditCardRepository creditCardRepository,
            ITransactionRepository transactionRepository,
            IFinancialConfigurationRepository financialConfigurationRepository)
        {
            _creditCardRepository = creditCardRepository;
            _transactionRepository = transactionRepository;
            _financialConfigurationRepository = financialConfigurationRepository;
        }

        private static string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return string.Empty;
            }

            var normalizedCardNumber = cardNumber
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty);

            if (normalizedCardNumber.Length < 4)
            {
                return normalizedCardNumber;
            }

            var lastFourDigits = normalizedCardNumber[^4..];

            return $"**** **** **** {lastFourDigits}";
        }

        public async Task<CreditCardStatementDto> Handle(
    GetCreditCardStatementQuery request,
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
                .GetByCreditCardIdAsync(request.CreditCardId);

            var interestConfiguration = await _financialConfigurationRepository
                .GetByCodeAsync(ConfigurationCodes.InterestPercentage);

            var minimumPaymentConfiguration = await _financialConfigurationRepository
                .GetByCodeAsync(ConfigurationCodes.MinimumPaymentPercentage);

            if (interestConfiguration is null ||
                minimumPaymentConfiguration is null)
            {
                throw new BusinessException(
                        "No se encontraron las configuraciones financieras requeridas.");
            }

            var transactionList = transactions.ToList();

            var totalPurchases = transactionList
                .Where(x => x.TransactionType == TransactionType.Purchase)
                .Sum(x => x.Amount);

            var totalPayments = transactionList
                .Where(x => x.TransactionType == TransactionType.Payment)
                .Sum(x => x.Amount);

            var usedBalance = totalPurchases - totalPayments;

            var availableBalance = creditCard.CreditLimit - usedBalance;

            var currentDate = DateTime.Now;

            var currentMonthPurchases = transactionList
                .Where(x =>
                    x.TransactionType == TransactionType.Purchase &&
                    x.TransactionDate.Year == currentDate.Year &&
                    x.TransactionDate.Month == currentDate.Month)
                .Sum(x => x.Amount);

            var previousMonthDate = currentDate.AddMonths(-1);

            var previousMonthPurchases = transactionList
                .Where(x =>
                    x.TransactionType == TransactionType.Purchase &&
                    x.TransactionDate.Year == previousMonthDate.Year &&
                    x.TransactionDate.Month == previousMonthDate.Month)
                .Sum(x => x.Amount);

            var interestPercentage = interestConfiguration.Value;

            var minimumPaymentPercentage = minimumPaymentConfiguration.Value;

            var bonusInterest = usedBalance * (interestPercentage / 100m);

            var minimumPayment = usedBalance * (minimumPaymentPercentage / 100m);

            var totalPayment = usedBalance;

            var cashPaymentWithInterest = usedBalance + bonusInterest;

            return new CreditCardStatementDto
            {
                CreditCardId = creditCard.Id,
                CardHolderName = creditCard.CardHolder?.FullName ?? string.Empty,
                CardNumber = MaskCardNumber(creditCard.CardNumber),

                CreditLimit = Math.Round(creditCard.CreditLimit, 2),
                UsedBalance = Math.Round(usedBalance, 2),
                AvailableBalance = Math.Round(availableBalance, 2),

                CurrentMonthPurchases = Math.Round(currentMonthPurchases, 2),
                PreviousMonthPurchases = Math.Round(previousMonthPurchases, 2),

                InterestPercentage = interestPercentage,
                BonusInterest = Math.Round(bonusInterest, 2),

                MinimumPaymentPercentage = minimumPaymentPercentage,
                MinimumPayment = Math.Round(minimumPayment, 2),

                TotalPayment = Math.Round(totalPayment, 2),
                CashPaymentWithInterest = Math.Round(cashPaymentWithInterest, 2)
            };
        }
    }
}