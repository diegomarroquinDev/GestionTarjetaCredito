using GestionTarjetaCredito.Application.Features.Configurations.Commands.UpdateFinancialConfiguration;
using GestionTarjetaCredito.Application.Features.Configurations.Queries.GetFinancialConfigurations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GestionTarjetaCredito.Api.Models.Responses;
using GestionTarjetaCredito.Application.DTOs;


namespace GestionTarjetaCredito.Api.Controllers
{
    [ApiController]
    [Route("api/configurations")]
    [ProducesResponseType(typeof(IEnumerable<FinancialConfigurationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
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