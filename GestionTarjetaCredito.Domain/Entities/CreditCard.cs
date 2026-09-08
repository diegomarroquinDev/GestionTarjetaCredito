namespace GestionTarjetaCredito.Domain.Entities
{
    public class CreditCard
    {
        public int Id { get; set; }

        public int CardHolderId { get; set; }

        public string CardNumber { get; set; } = string.Empty;

        public decimal CreditLimit { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public CardHolder? CardHolder { get; set; }
    }
}