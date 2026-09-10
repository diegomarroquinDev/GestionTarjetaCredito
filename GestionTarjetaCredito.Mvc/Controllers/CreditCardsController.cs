using GestionTarjetaCredito.Mvc.Models;
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

        [HttpGet]
        public IActionResult CreatePurchase(int id = 1)
        {
            var model = new CreatePurchaseViewModel
            {
                CreditCardId = id,
                TransactionDate = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePurchase(
            CreatePurchaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _creditCardApiService
                    .CreatePurchaseAsync(model);

                TempData["SuccessMessage"] =
                    $"Compra registrada correctamente. Transacción #{result?.TransactionId}.";

                return RedirectToAction(
                    nameof(Statement),
                    new
                    {
                        id = model.CreditCardId
                    });
            }
            catch (InvalidOperationException exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible registrar la compra.");

                ViewBag.ApiError = exception.Message;

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CreatePayment(int id = 1)
        {
            var model = new CreatePaymentViewModel
            {
                CreditCardId = id,
                TransactionDate = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePayment(
            CreatePaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _creditCardApiService
                    .CreatePaymentAsync(model);

                TempData["SuccessMessage"] =
                    $"Pago registrado correctamente. Transacción #{result?.TransactionId}.";

                return RedirectToAction(
                    nameof(Statement),
                    new
                    {
                        id = model.CreditCardId
                    });
            }
            catch (InvalidOperationException exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible registrar el pago.");

                ViewBag.ApiError = exception.Message;

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Configurations()
        {
            var model = await _creditCardApiService
                .GetFinancialConfigurationsAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateConfiguration(
            FinancialConfigurationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var configurations = await _creditCardApiService
                    .GetFinancialConfigurationsAsync();

                return View(
                    "Configurations",
                    configurations);
            }

            try
            {
                await _creditCardApiService
                    .UpdateFinancialConfigurationAsync(model);

                TempData["SuccessMessage"] =
                    "Configuración actualizada correctamente.";

                return RedirectToAction(
                    nameof(Configurations));
            }
            catch (InvalidOperationException exception)
            {
                TempData["ErrorMessage"] = exception.Message;

                return RedirectToAction(
                    nameof(Configurations));
            }
        }
    }
}