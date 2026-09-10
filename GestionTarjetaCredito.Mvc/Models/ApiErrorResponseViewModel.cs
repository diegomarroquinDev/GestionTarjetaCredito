namespace GestionTarjetaCredito.Mvc.Models
{
    public class ApiErrorResponseViewModel
    {
        public int Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public object? Errors { get; set; }
    }
}