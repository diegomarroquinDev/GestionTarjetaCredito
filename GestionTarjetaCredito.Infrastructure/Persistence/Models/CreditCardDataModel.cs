namespace GestionTarjetaCredito.Infrastructure.Persistence.Models
{
    internal class CreditCardDataModel
    {
        public int Id { get; set; }

        public int CardHolderId { get; set; }

        public string CardNumber { get; set; } = string.Empty;

        public decimal CreditLimit { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CardHolderName { get; set; } = string.Empty;

        public DateTime CardHolderCreatedDate { get; set; }
    }
}