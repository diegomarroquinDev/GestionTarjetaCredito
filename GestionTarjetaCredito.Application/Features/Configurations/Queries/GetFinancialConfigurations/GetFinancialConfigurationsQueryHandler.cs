using AutoMapper;
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

        private readonly IMapper _mapper;

        public GetFinancialConfigurationsQueryHandler(
            IFinancialConfigurationRepository financialConfigurationRepository,
            IMapper mapper)
        {
            _financialConfigurationRepository = financialConfigurationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FinancialConfigurationDto>> Handle(
            GetFinancialConfigurationsQuery request,
            CancellationToken cancellationToken)
        {
            var configurations = await _financialConfigurationRepository
                .GetAllAsync();

            return _mapper.Map<IEnumerable<FinancialConfigurationDto>>(
                configurations);
        }
    }
}