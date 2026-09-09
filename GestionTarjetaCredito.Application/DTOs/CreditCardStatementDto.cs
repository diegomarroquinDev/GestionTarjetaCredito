namespace GestionTarjetaCredito.Application.DTOs
{
    public class CreditCardStatementDto
    {
        public int CreditCardId { get; set; }

        public string CardHolderName { get; set; } = string.Empty;

        public string CardNumber { get; set; } = string.Empty;

        public decimal CreditLimit { get; set; }

        public decimal UsedBalance { get; set; }

        public decimal AvailableBalance { get; set; }

        public decimal CurrentMonthPurchases { get; set; }

        public decimal PreviousMonthPurchases { get; set; }

        public decimal InterestPercentage { get; set; }

        public decimal BonusInterest { get; set; }

        public decimal MinimumPaymentPercentage { get; set; }

        public decimal MinimumPayment { get; set; }

        public decimal TotalPayment { get; set; }

        public decimal CashPaymentWithInterest { get; set; }
    }
}