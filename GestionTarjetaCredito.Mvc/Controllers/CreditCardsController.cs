using GestionTarjetaCredito.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionTarjetaCredito.Mvc.Controllers
{
    public class CreditCardsController : Controller
    {
        private readonly CreditCardApiService _creditCardApiService;

        public CreditCardsController(
            CreditCardApiService creditCardApiService)
        {
            _creditCardApiService = creditCardApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Statement(
            int id = 1)
        {
            var model = await _creditCardApiService
                .GetStatementAsync(id);

            if (model is null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Transactions(
            int id = 1,
            int? year = null,
            int? month = null)
        {
            var currentDate = DateTime.Now;

            var selectedYear = year ?? currentDate.Year;
            var selectedMonth = month ?? currentDate.Month;

            var model = await _creditCardApiService
                .GetMonthlyTransactionsAsync(
                    id,
                    selectedYear,
                    selectedMonth);

            ViewBag.CreditCardId = id;
            ViewBag.Year = selectedYear;
            ViewBag.Month = selectedMonth;

            return View(model);
        }
    }
}