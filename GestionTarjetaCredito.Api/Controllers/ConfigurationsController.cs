using GestionTarjetaCredito.Application.Features.Configurations.Commands.UpdateFinancialConfiguration;
using GestionTarjetaCredito.Application.Features.Configurations.Queries.GetFinancialConfigurations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionTarjetaCredito.Api.Controllers
{
    [ApiController]
    [Route("api/configurations")]
    public class ConfigurationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConfigurationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetFinancialConfigurationsQuery(),
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateFinancialConfigurationCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;

            await _mediator.Send(
                command,
                cancellationToken);

            return NoContent();
        }
    }
}