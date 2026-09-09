using Dapper;
using GestionTarjetaCredito.Application.Interfaces;
using GestionTarjetaCredito.Domain.Entities;
using GestionTarjetaCredito.Infrastructure.Persistence;
using System.Data;

namespace GestionTarjetaCredito.Infrastructure.Repositories
{
    public class FinancialConfigurationRepository
        : IFinancialConfigurationRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public FinancialConfigurationRepository(
            SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<FinancialConfiguration?> GetByCodeAsync(
            string code)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                Code = code
            };

            return await connection
                .QueryFirstOrDefaultAsync<FinancialConfiguration>(
                    "sp_FinancialConfiguration_GetByCode",
                    parameters,
                    commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<FinancialConfiguration>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<FinancialConfiguration>(
                "sp_FinancialConfiguration_GetAll",
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(
            FinancialConfiguration configuration)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                configuration.Id,
                configuration.Value,
                configuration.UpdatedDate
            };

            await connection.ExecuteAsync(
                "sp_FinancialConfiguration_Update",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}