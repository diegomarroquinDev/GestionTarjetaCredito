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
    }
}