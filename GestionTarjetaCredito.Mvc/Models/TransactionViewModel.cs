namespace GestionTarjetaCredito.Mvc.Models
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}