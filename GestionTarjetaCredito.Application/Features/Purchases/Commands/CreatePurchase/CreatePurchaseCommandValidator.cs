using FluentValidation;

namespace GestionTarjetaCredito.Application.Features.Purchases.Commands.CreatePurchase
{
    public class CreatePurchaseCommandValidator
        : AbstractValidator<CreatePurchaseCommand>
    {
        public CreatePurchaseCommandValidator()
        {
            RuleFor(x => x.CreditCardId)
                .GreaterThan(0)
                .WithMessage("El identificador de la tarjeta debe ser mayor que cero.");

            RuleFor(x => x.TransactionDate)
                .NotEmpty()
                .WithMessage("La fecha de la compra es requerida.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("La descripción de la compra es requerida.")
                .MaximumLength(200)
                .WithMessage("La descripción no puede exceder los 200 caracteres.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("El monto de la compra debe ser mayor que cero.");
        }
    }
}