using Dapper;
using GestionTarjetaCredito.Application.Interfaces;
using GestionTarjetaCredito.Domain.Entities;
using GestionTarjetaCredito.Infrastructure.Persistence;
using System.Data;

namespace GestionTarjetaCredito.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public TransactionRepository(
            SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Transaction>> GetByCreditCardIdAsync(
            int creditCardId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                CreditCardId = creditCardId
            };

            var transactions = await connection.QueryAsync<Transaction>(
                "sp_Transaction_GetByCreditCardId",
                parameters,
                commandType: CommandType.StoredProcedure);

            return transactions;
        }

        public async Task<IEnumerable<Transaction>> GetMonthlyTransactionsAsync(
            int creditCardId,
            int year,
            int month)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                CreditCardId = creditCardId,
                Year = year,
                Month = month
            };

            var transactions = await connection.QueryAsync<Transaction>(
                "sp_Transaction_GetMonthly",
                parameters,
                commandType: CommandType.StoredProcedure);

            return transactions;
        }

        public async Task<int> CreatePurchaseAsync(
         Transaction transaction,
        IUnitOfWork unitOfWork)
        {
            if (unitOfWork is not UnitOfWork sqlUnitOfWork ||
                sqlUnitOfWork.Connection is null ||
                sqlUnitOfWork.Transaction is null)
            {
                throw new InvalidOperationException(
                    "La transacción de base de datos no ha sido iniciada.");
            }

            var parameters = new
            {
                transaction.CreditCardId,
                transaction.TransactionDate,
                transaction.Description,
                transaction.Amount
            };

            var transactionId = await sqlUnitOfWork.Connection
                .QuerySingleAsync<int>(
                    "sp_Transaction_CreatePurchase",
                    parameters,
                    transaction: sqlUnitOfWork.Transaction,
                    commandType: CommandType.StoredProcedure);

            return transactionId;
        }
    }

}