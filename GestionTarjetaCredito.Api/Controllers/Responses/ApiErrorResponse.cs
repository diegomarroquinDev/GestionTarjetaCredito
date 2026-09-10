namespace GestionTarjetaCredito.Api.Models.Responses
{
    public class ApiErrorResponse
    {
        public int Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public object? Errors { get; set; }
    }
}