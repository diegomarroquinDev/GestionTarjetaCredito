using GestionTarjetaCredito.Application.Features.CreditCards.Queries.GetCreditCardStatement;
using GestionTarjetaCredito.Application.Features.Purchases.Commands.CreatePurchase;
using GestionTarjetaCredito.Application.Features.Payments.Commands.CreatePayment;
using GestionTarjetaCredito.Application.Features.Transactions.Queries.GetMonthlyTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GestionTarjetaCredito.Api.Models.Responses;
using GestionTarjetaCredito.Application.DTOs;


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
        [ProducesResponseType(typeof(CreditCardStatementDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
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

        [HttpPost("{id:int}/purchases")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePurchase(
            int id,
            [FromBody] CreatePurchaseCommand command,
            CancellationToken cancellationToken)
        {
            command.CreditCardId = id;

            var transactionId = await _mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetStatement),
                new { id },
                new
                {
                    transactionId
                });
        }

        [HttpPost("{id:int}/payments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePayment(
            int id,
            [FromBody] CreatePaymentCommand command,
            CancellationToken cancellationToken)
        {
            command.CreditCardId = id;

            var transactionId = await _mediator.Send(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetStatement),
                new { id },
                new
                {
                    transactionId
                });
        }

        [HttpGet("{id:int}/transactions")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMonthlyTransactions(
            int id,
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetMonthlyTransactionsQuery
                {
                    CreditCardId = id,
                    Year = year,
                    Month = month
                },
                cancellationToken);

            return Ok(result);
        }
    }
}