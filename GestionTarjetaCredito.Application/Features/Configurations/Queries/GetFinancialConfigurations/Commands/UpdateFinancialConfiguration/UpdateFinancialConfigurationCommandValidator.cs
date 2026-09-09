using FluentValidation;

namespace GestionTarjetaCredito.Application.Features.Configurations.Commands.UpdateFinancialConfiguration
{
    public class UpdateFinancialConfigurationCommandValidator
        : AbstractValidator<UpdateFinancialConfigurationCommand>
    {
        public UpdateFinancialConfigurationCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El identificador de la configuración debe ser mayor que cero.");

            RuleFor(x => x.Value)
                .InclusiveBetween(0, 100)
                .WithMessage("El valor debe estar entre 0 y 100.");
        }
    }
}