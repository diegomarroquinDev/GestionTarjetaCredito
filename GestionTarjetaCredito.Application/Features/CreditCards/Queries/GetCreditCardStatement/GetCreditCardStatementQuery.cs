using GestionTarjetaCredito.Application.DTOs;
using MediatR;

namespace GestionTarjetaCredito.Application.Features.CreditCards.Queries.GetCreditCardStatement
{
    public class GetCreditCardStatementQuery : IRequest<CreditCardStatementDto>
    {
        public int CreditCardId { get; set; }
    }
}