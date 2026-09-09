using FluentValidation;

namespace GestionTarjetaCredito.Application.Features.CreditCards.Queries.GetCreditCardStatement
{
    public class GetCreditCardStatementQueryValidator
        : AbstractValidator<GetCreditCardStatementQuery>
    {
        public GetCreditCardStatementQueryValidator()
        {
            RuleFor(x => x.CreditCardId)
                .GreaterThan(0)
                .WithMessage("El identificador de la tarjeta debe ser mayor que cero.");
        }
    }
}