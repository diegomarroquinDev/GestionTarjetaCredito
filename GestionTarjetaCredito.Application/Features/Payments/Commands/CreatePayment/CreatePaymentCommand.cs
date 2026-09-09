using MediatR;

namespace GestionTarjetaCredito.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<int>
    {
        public int CreditCardId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }
    }
}