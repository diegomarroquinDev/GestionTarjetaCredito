using GestionTarjetaCredito.Application.Interfaces;
using Microsoft.Data.SqlClient;

namespace GestionTarjetaCredito.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnectionFactory _connectionFactory;

        private SqlConnection? _connection;
        private SqlTransaction? _transaction;

        public SqlConnection? Connection => _connection;

        public SqlTransaction? Transaction => _transaction;

        public UnitOfWork(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task BeginTransactionAsync()
        {
            if (_connection is not null)
            {
                throw new InvalidOperationException(
                    "Ya existe una transacción activa.");
            }

            _connection = (SqlConnection)_connectionFactory.CreateConnection();

            await _connection.OpenAsync();

            _transaction = _connection.BeginTransaction();
        }

        public Task CommitAsync()
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException(
                    "No existe una transacción activa para confirmar.");
            }

            _transaction.Commit();

            DisposeTransaction();

            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            if (_transaction is null)
            {
                return Task.CompletedTask;
            }

            _transaction.Rollback();

            DisposeTransaction();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            DisposeTransaction();

            GC.SuppressFinalize(this);
        }

        private void DisposeTransaction()
        {
            _transaction?.Dispose();
            _connection?.Dispose();

            _transaction = null;
            _connection = null;
        }
    }
}