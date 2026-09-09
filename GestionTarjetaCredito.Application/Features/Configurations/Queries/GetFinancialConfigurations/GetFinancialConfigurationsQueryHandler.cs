using GestionTarjetaCredito.Application.DTOs;
using GestionTarjetaCredito.Application.Interfaces;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.Configurations.Queries.GetFinancialConfigurations
{
    public class GetFinancialConfigurationsQueryHandler
        : IRequestHandler<
            GetFinancialConfigurationsQuery,
            IEnumerable<FinancialConfigurationDto>>
    {
        private readonly IFinancialConfigurationRepository
            _financialConfigurationRepository;

        public GetFinancialConfigurationsQueryHandler(
            IFinancialConfigurationRepository financialConfigurationRepository)
        {
            _financialConfigurationRepository = financialConfigurationRepository;
        }

        public async Task<IEnumerable<FinancialConfigurationDto>> Handle(
            GetFinancialConfigurationsQuery request,
            CancellationToken cancellationToken)
        {
            var configurations = await _financialConfigurationRepository
                .GetAllAsync();

            return configurations
                .Select(x => new FinancialConfigurationDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Value = x.Value,
                    Description = x.Description,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToList();
        }
    }
}