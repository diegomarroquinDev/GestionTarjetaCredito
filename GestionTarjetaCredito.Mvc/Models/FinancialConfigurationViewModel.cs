using System.ComponentModel.DataAnnotations;

namespace GestionTarjetaCredito.Mvc.Models
{
    public class FinancialConfigurationViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        [Display(Name = "Valor")]
        [Range(
            0,
            100,
            ErrorMessage = "El valor debe estar entre 0 y 100.")]
        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}