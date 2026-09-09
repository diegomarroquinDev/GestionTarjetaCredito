using GestionTarjetaCredito.Domain.Entities;

namespace GestionTarjetaCredito.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetByCreditCardIdAsync(int creditCardId);

        Task<IEnumerable<Transaction>> GetMonthlyTransactionsAsync(
            int creditCardId,
            int year,
            int month);

        Task<int> CreatePurchaseAsync(
            Transaction transaction,
            IUnitOfWork unitOfWork);

        Task<int> CreatePaymentAsync(
            Transaction transaction,
            IUnitOfWork unitOfWork);
    }
}