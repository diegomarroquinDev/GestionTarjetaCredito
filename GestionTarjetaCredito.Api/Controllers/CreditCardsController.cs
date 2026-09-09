using GestionTarjetaCredito.Application.Features.CreditCards.Queries.GetCreditCardStatement;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionTarjetaCredito.Api.Controllers
{
    [ApiController]
    [Route("api/credit-cards")]
    public class CreditCardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreditCardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:int}/statement")]
        public async Task<IActionResult> GetStatement(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetCreditCardStatementQuery
                {
                    CreditCardId = id
                },
                cancellationToken);

            return Ok(result);
        }
    }
}