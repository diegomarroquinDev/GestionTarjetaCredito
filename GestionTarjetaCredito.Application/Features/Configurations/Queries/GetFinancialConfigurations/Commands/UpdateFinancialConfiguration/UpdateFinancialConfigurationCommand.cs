using MediatR;

namespace GestionTarjetaCredito.Application.Features.Configurations.Commands.UpdateFinancialConfiguration
{
    public class UpdateFinancialConfigurationCommand : IRequest
    {
        public int Id { get; set; }

        public decimal Value { get; set; }
    }
}