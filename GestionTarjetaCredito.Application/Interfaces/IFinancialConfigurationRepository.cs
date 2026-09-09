using GestionTarjetaCredito.Domain.Entities;

namespace GestionTarjetaCredito.Application.Interfaces
{
    public interface IFinancialConfigurationRepository
    {
        Task<FinancialConfiguration?> GetByCodeAsync(string code);

        Task<IEnumerable<FinancialConfiguration>> GetAllAsync();

        Task UpdateAsync(FinancialConfiguration configuration);
    }
}