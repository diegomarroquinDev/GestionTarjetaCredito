using System.ComponentModel.DataAnnotations;

namespace GestionTarjetaCredito.Mvc.Models
{
    public class CreatePurchaseViewModel
    {
        public int CreditCardId { get; set; }

        [Required(ErrorMessage = "La fecha de la compra es requerida.")]
        [Display(Name = "Fecha de compra")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La descripción es requerida.")]
        [StringLength(
            200,
            ErrorMessage = "La descripción no puede exceder los 200 caracteres.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; } = string.Empty;

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "El monto debe ser mayor que cero.")]
        [Display(Name = "Monto")]
        public decimal Amount { get; set; }
    }
}