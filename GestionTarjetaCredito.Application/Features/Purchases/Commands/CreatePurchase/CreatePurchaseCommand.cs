using MediatR;

namespace GestionTarjetaCredito.Application.Features.Purchases.Commands.CreatePurchase
{
    public class CreatePurchaseCommand : IRequest<int>
    {
        public int CreditCardId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}