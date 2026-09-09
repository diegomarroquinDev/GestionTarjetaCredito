using GestionTarjetaCredito.Application.Interfaces;
using GestionTarjetaCredito.Infrastructure.Persistence;
using GestionTarjetaCredito.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionTarjetaCredito.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<SqlConnectionFactory>();

            services.AddScoped<ICreditCardRepository, CreditCardRepository>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<
                IFinancialConfigurationRepository,
                FinancialConfigurationRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}