using FluentValidation;

namespace GestionTarjetaCredito.Application.Features.Transactions.Queries.GetMonthlyTransactions
{
    public class GetMonthlyTransactionsQueryValidator
        : AbstractValidator<GetMonthlyTransactionsQuery>
    {
        public GetMonthlyTransactionsQueryValidator()
        {
            RuleFor(x => x.CreditCardId)
                .GreaterThan(0)
                .WithMessage("El identificador de la tarjeta debe ser mayor que cero.");

            RuleFor(x => x.Year)
                .GreaterThanOrEqualTo(2000)
                .WithMessage("El año debe ser válido.");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12)
                .WithMessage("El mes debe estar entre 1 y 12.");
        }
    }
}