using GestionTarjetaCredito.Domain.Entities;

namespace GestionTarjetaCredito.Application.Interfaces
{
    public interface ICreditCardRepository
    {
        Task<CreditCard?> GetByIdAsync(int creditCardId);
    }
}   