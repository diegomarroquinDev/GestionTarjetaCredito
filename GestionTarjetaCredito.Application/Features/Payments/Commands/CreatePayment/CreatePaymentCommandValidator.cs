using FluentValidation;

namespace GestionTarjetaCredito.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandValidator
        : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(x => x.CreditCardId)
                .GreaterThan(0)
                .WithMessage("El identificador de la tarjeta debe ser mayor que cero.");

            RuleFor(x => x.TransactionDate)
                .NotEmpty()
                .WithMessage("La fecha del pago es requerida.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("El monto del pago debe ser mayor que cero.");
        }
    }
}