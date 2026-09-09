using Dapper;
using GestionTarjetaCredito.Application.Interfaces;
using GestionTarjetaCredito.Domain.Entities;
using GestionTarjetaCredito.Infrastructure.Persistence;
using GestionTarjetaCredito.Infrastructure.Persistence.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionTarjetaCredito.Infrastructure.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public CreditCardRepository(
            SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CreditCard?> GetByIdAsync(int creditCardId)
        {
            using var connection = _connectionFactory.CreateConnection();

            if (connection is null)
            {
                throw new InvalidOperationException(
                    "SqlConnectionFactory devolvió una conexión nula.");
            }

            if (connection is SqlConnection sqlConnection)
            {
                await sqlConnection.OpenAsync();
            }

            var parameters = new
            {
                CreditCardId = creditCardId
            };

            var result = await connection
                .QueryFirstOrDefaultAsync<CreditCardDataModel>(
                    "sp_CreditCard_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure);

            if (result is null)
            {
                return null;
            }

            return new CreditCard
            {
                Id = result.Id,
                CardHolderId = result.CardHolderId,
                CardNumber = result.CardNumber,
                CreditLimit = result.CreditLimit,
                IsActive = result.IsActive,
                CreatedDate = result.CreatedDate,

                CardHolder = new CardHolder
                {
                    Id = result.CardHolderId,
                    FullName = result.CardHolderName,
                    CreatedDate = result.CardHolderCreatedDate
                }
            };
        }
    }
}