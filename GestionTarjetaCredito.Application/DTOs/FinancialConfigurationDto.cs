namespace GestionTarjetaCredito.Application.DTOs
{
    public class FinancialConfigurationDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}