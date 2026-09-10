using System.ComponentModel.DataAnnotations;

namespace GestionTarjetaCredito.Mvc.Models
{
    public class FinancialConfigurationViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string DisplayName
        {
            get
            {
                return Code switch
                {
                    "INTEREST_PERCENTAGE" => "Porcentaje de interés",
                    "MIN_PAYMENT_PERCENTAGE" => "Porcentaje de pago mínimo",
                    _ => Code
                };
            }
        }
    }
}