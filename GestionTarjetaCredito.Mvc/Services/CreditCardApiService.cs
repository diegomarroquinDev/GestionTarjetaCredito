using GestionTarjetaCredito.Mvc.Models;
using GestionTarjetaCredito.Mvc.Services;
using System.Net.Http.Json;

namespace GestionTarjetaCredito.Mvc.Services
{
    public class CreditCardApiService
    {
        private readonly HttpClient _httpClient;

        public CreditCardApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreditCardStatementViewModel?> GetStatementAsync(
            int creditCardId)
        {
            return await _httpClient.GetFromJsonAsync<CreditCardStatementViewModel>(
                $"api/credit-cards/{creditCardId}/statement");
        }

        public async Task<IEnumerable<TransactionViewModel>> GetMonthlyTransactionsAsync(
        int creditCardId,
        int year,
        int month)
        {
            var transactions =
                await _httpClient.GetFromJsonAsync<IEnumerable<TransactionViewModel>>(
                    $"api/credit-cards/{creditCardId}/transactions?year={year}&month={month}");

            return transactions ?? Enumerable.Empty<TransactionViewModel>();
        }

        public async Task<CreateTransactionResponseViewModel?> CreatePurchaseAsync(
            CreatePurchaseViewModel model)
        {
            var request = new
            {
                transactionDate = model.TransactionDate,
                description = model.Description,
                amount = model.Amount
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"api/credit-cards/{model.CreditCardId}/purchases",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new InvalidOperationException(error);
            }

            return await response.Content
                .ReadFromJsonAsync<CreateTransactionResponseViewModel>();
        }

        public async Task<CreateTransactionResponseViewModel?> CreatePaymentAsync(
        CreatePaymentViewModel model)
        {
            var request = new
            {
                transactionDate = model.TransactionDate,
                amount = model.Amount
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"api/credit-cards/{model.CreditCardId}/payments",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new InvalidOperationException(error);
            }

            return await response.Content
                .ReadFromJsonAsync<CreateTransactionResponseViewModel>();
        }

        public async Task<IEnumerable<FinancialConfigurationViewModel>>
            GetFinancialConfigurationsAsync()
        {
            var configurations =
                await _httpClient.GetFromJsonAsync<
                    IEnumerable<FinancialConfigurationViewModel>>(
                        "api/configurations");

            return configurations ??
                   Enumerable.Empty<FinancialConfigurationViewModel>();
        }

        public async Task UpdateFinancialConfigurationAsync(
            FinancialConfigurationViewModel model)
        {
            var request = new
            {
                value = model.Value
            };

            var response = await _httpClient.PutAsJsonAsync(
                $"api/configurations/{model.Id}",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new InvalidOperationException(error);
            }
        }
    }
}