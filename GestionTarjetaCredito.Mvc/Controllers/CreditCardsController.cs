using GestionTarjetaCredito.Mvc.Models;
using GestionTarjetaCredito.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using GestionTarjetaCredito.Mvc.Documents;
using QuestPDF.Fluent;

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
                    exception.Message);

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
                    exception.Message);

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

        [HttpGet]
        public async Task<IActionResult> ExportTransactionsExcel(
             int id = 1,
             int? year = null,
             int? month = null)
        {
            var selectedYear = year ?? DateTime.Now.Year;
            var selectedMonth = month ?? DateTime.Now.Month;

            var transactions = await _creditCardApiService
                .GetMonthlyTransactionsAsync(
                    id,
                    selectedYear,
                    selectedMonth);

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Movimientos");

            worksheet.Cell("A1").Value = "MOVIMIENTOS DE TARJETA DE CRÉDITO";
            worksheet.Range("A1:D1").Merge();

            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("A1").Style.Font.FontSize = 16;
            worksheet.Cell("A1").Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            worksheet.Cell("A2").Value =
                $"Tarjeta Id: {id} | Periodo: {selectedMonth:D2}/{selectedYear}";

            worksheet.Range("A2:D2").Merge();

            worksheet.Cell("A2").Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            worksheet.Cell("A4").Value = "Fecha";
            worksheet.Cell("B4").Value = "Tipo";
            worksheet.Cell("C4").Value = "Descripción";
            worksheet.Cell("D4").Value = "Monto";

            var headerRange = worksheet.Range("A4:D4");

            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            var row = 5;

            decimal totalPurchases = 0;
            decimal totalPayments = 0;

            foreach (var transaction in transactions)
            {
                var transactionType =
                    transaction.TransactionType == "Purchase"
                        ? "Compra"
                        : transaction.TransactionType == "Payment"
                            ? "Pago"
                            : transaction.TransactionType;

                worksheet.Cell(row, 1).Value =
                    transaction.TransactionDate;

                worksheet.Cell(row, 2).Value =
                    transactionType;

                worksheet.Cell(row, 3).Value =
                    transaction.Description;

                worksheet.Cell(row, 4).Value =
                    transaction.Amount;

                if (transaction.TransactionType == "Purchase")
                {
                    totalPurchases += transaction.Amount;
                }

                if (transaction.TransactionType == "Payment")
                {
                    totalPayments += transaction.Amount;
                }

                row++;
            }

            if (row > 5)
            {
                worksheet.Range($"A5:A{row - 1}")
                    .Style.DateFormat.Format = "dd/MM/yyyy HH:mm";

                worksheet.Range($"D5:D{row - 1}")
                    .Style.NumberFormat.Format = "$#,##0.00";
            }

            var summaryStartRow = row + 2;

            worksheet.Cell(summaryStartRow, 3).Value =
                "Total compras:";

            worksheet.Cell(summaryStartRow, 4).Value =
                totalPurchases;

            worksheet.Cell(summaryStartRow + 1, 3).Value =
                "Total pagos:";

            worksheet.Cell(summaryStartRow + 1, 4).Value =
                totalPayments;

            worksheet.Cell(summaryStartRow + 2, 3).Value =
                "Balance de movimientos:";

            worksheet.Cell(summaryStartRow + 2, 4).Value =
                totalPurchases - totalPayments;

            worksheet.Range(
                    summaryStartRow,
                    3,
                    summaryStartRow + 2,
                    3)
                .Style.Font.Bold = true;

            worksheet.Range(
                    summaryStartRow,
                    4,
                    summaryStartRow + 2,
                    4)
                .Style.Font.Bold = true;

            worksheet.Range(
                    summaryStartRow,
                    4,
                    summaryStartRow + 2,
                    4)
                .Style.NumberFormat.Format = "$#,##0.00";

            worksheet.Columns().AdjustToContents();

            worksheet.Column(1).Width = 22;
            worksheet.Column(2).Width = 15;
            worksheet.Column(3).Width = 45;
            worksheet.Column(4).Width = 18;

            worksheet.SheetView.FreezeRows(4);

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            var fileName =
                $"Movimientos_Tarjeta_{id}_{selectedYear}_{selectedMonth:D2}.xlsx";

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        [HttpGet]
        public async Task<IActionResult> ExportStatementPdf(
            int id = 1,
            int? year = null,
            int? month = null)
        {
            var selectedYear = year ?? DateTime.Now.Year;
            var selectedMonth = month ?? DateTime.Now.Month;

            var statement = await _creditCardApiService
                .GetStatementAsync(id);

            if (statement is null)
            {
                return NotFound();
            }

            var transactions = await _creditCardApiService
                .GetMonthlyTransactionsAsync(
                    id,
                    selectedYear,
                    selectedMonth);

            var document =
                new CreditCardStatementPdfDocument(
                    statement,
                    transactions);

            var pdf = document.GeneratePdf();

            var fileName =
                $"EstadoCuenta_Tarjeta_{id}_{selectedYear}_{selectedMonth:D2}.pdf";

            return File(
                pdf,
                "application/pdf",
                fileName);
        }
    }
}