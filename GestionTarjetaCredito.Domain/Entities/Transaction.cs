using GestionTarjetaCredito.Domain.Enums;

namespace GestionTarjetaCredito.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public int CreditCardId { get; set; }

        public TransactionType TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime CreatedDate { get; set; }

        public CreditCard? CreditCard { get; set; }
    }
}