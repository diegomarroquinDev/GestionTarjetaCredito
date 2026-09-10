using System.ComponentModel.DataAnnotations;

namespace GestionTarjetaCredito.Mvc.Models
{
    public class CreatePaymentViewModel
    {
        public int CreditCardId { get; set; }

        [Required(ErrorMessage = "La fecha del pago es requerida.")]
        [Display(Name = "Fecha de pago")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "El monto debe ser mayor que cero.")]
        [Display(Name = "Monto")]
        public decimal Amount { get; set; }
    }
}