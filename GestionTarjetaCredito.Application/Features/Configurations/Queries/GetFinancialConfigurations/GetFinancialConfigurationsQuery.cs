using GestionTarjetaCredito.Application.DTOs;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.Configurations.Queries.GetFinancialConfigurations
{
    public class GetFinancialConfigurationsQuery
        : IRequest<IEnumerable<FinancialConfigurationDto>>
    {
    }
}