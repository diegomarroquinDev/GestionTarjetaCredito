using GestionTarjetaCredito.Application.Common.Exceptions;
using GestionTarjetaCredito.Application.Interfaces;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.Configurations.Commands.UpdateFinancialConfiguration
{
    public class UpdateFinancialConfigurationCommandHandler
        : IRequestHandler<UpdateFinancialConfigurationCommand>
    {
        private readonly IFinancialConfigurationRepository
            _financialConfigurationRepository;

        public UpdateFinancialConfigurationCommandHandler(
            IFinancialConfigurationRepository financialConfigurationRepository)
        {
            _financialConfigurationRepository = financialConfigurationRepository;
        }

        public async Task Handle(
            UpdateFinancialConfigurationCommand request,
            CancellationToken cancellationToken)
        {
            var configurations = await _financialConfigurationRepository
                .GetAllAsync();

            var configuration = configurations
                .FirstOrDefault(x => x.Id == request.Id);

            if (configuration is null)
            {
                throw new NotFoundException(
                    $"No se encontró la configuración con Id {request.Id}.");
            }

            configuration.Value = request.Value;
            configuration.UpdatedDate = DateTime.Now;

            await _financialConfigurationRepository
                .UpdateAsync(configuration);
        }
    }
}