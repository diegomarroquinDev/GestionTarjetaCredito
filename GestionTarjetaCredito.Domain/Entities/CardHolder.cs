namespace GestionTarjetaCredito.Domain.Entities
{
    public class CardHolder
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}