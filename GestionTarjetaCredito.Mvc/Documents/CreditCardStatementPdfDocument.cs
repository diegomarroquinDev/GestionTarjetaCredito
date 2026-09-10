using GestionTarjetaCredito.Mvc.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GestionTarjetaCredito.Mvc.Documents
{
    public class CreditCardStatementPdfDocument : IDocument
    {
        private readonly CreditCardStatementViewModel _statement;
        private readonly IEnumerable<TransactionViewModel> _transactions;

        public CreditCardStatementPdfDocument(
            CreditCardStatementViewModel statement,
            IEnumerable<TransactionViewModel> transactions)
        {
            _statement = statement;
            _transactions = transactions;
        }

        public DocumentMetadata GetMetadata()
        {
            return DocumentMetadata.Default;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(35);

                page.DefaultTextStyle(style =>
                    style.FontSize(10));

                page.Header()
                    .Element(ComposeHeader);

                page.Content()
                    .PaddingVertical(20)
                    .Element(ComposeContent);

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Generado el ");
                        text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                        text.Span("  |  Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item()
                        .Text("BANCA")
                        .FontSize(18)
                        .Bold()
                        .FontColor("#0B2C5F");

                    column.Item()
                        .Text("Estado de cuenta de tarjeta de crédito")
                        .FontSize(12)
                        .FontColor("#667085");
                });

                row.ConstantItem(120)
                    .AlignRight()
                    .Text(DateTime.Now.ToString("MMMM yyyy"))
                    .FontSize(11)
                    .Bold()
                    .FontColor("#C8102E");
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(18);

                column.Item()
                    .Element(ComposeCardInformation);

                column.Item()
                    .Element(ComposeFinancialSummary);

                column.Item()
                    .Element(ComposePaymentSummary);

                column.Item()
                    .Element(ComposeTransactions);
            });
        }

        private void ComposeCardInformation(IContainer container)
        {
            container
                .Background("#0B2C5F")
                .Padding(20)
                .Column(column =>
                {
                    column.Spacing(6);

                    column.Item()
                        .Text("Tarjeta de crédito")
                        .FontSize(11)
                        .FontColor(Colors.White);

                    column.Item()
                        .Text(_statement.CardNumber)
                        .FontSize(18)
                        .Bold()
                        .FontColor(Colors.White);

                    column.Item()
                        .PaddingTop(10)
                        .Text(_statement.CardHolderName)
                        .FontSize(11)
                        .FontColor(Colors.White);

                    column.Item()
                        .Text($"Límite de crédito: {_statement.CreditLimit:C2}")
                        .FontColor(Colors.White);
                });
        }

        private void ComposeFinancialSummary(IContainer container)
        {
            container.Column(column =>
            {
                column.Item()
                    .Text("Resumen financiero")
                    .FontSize(14)
                    .Bold()
                    .FontColor("#17202A");

                column.Item()
                    .PaddingTop(10)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        SummaryCell(
                            table,
                            "Saldo utilizado",
                            _statement.UsedBalance.ToString("C2"));

                        SummaryCell(
                            table,
                            "Crédito disponible",
                            _statement.AvailableBalance.ToString("C2"));

                        SummaryCell(
                            table,
                            "Límite de crédito",
                            _statement.CreditLimit.ToString("C2"));

                        SummaryCell(
                            table,
                            "Compras mes actual",
                            _statement.CurrentMonthPurchases.ToString("C2"));

                        SummaryCell(
                            table,
                            "Compras mes anterior",
                            _statement.PreviousMonthPurchases.ToString("C2"));

                        SummaryCell(
                            table,
                            "Interés",
                            $"{_statement.InterestPercentage:0.00}%");
                    });
            });
        }

        private static void SummaryCell(
            TableDescriptor table,
            string title,
            string value)
        {
            table.Cell()
                .Border(1)
                .BorderColor("#E4E7EC")
                .Padding(12)
                .Column(column =>
                {
                    column.Item()
                        .Text(title)
                        .FontSize(9)
                        .FontColor("#667085");

                    column.Item()
                        .PaddingTop(4)
                        .Text(value)
                        .FontSize(13)
                        .Bold()
                        .FontColor("#17202A");
                });
        }

        private void ComposePaymentSummary(IContainer container)
        {
            container.Column(column =>
            {
                column.Item()
                    .Text("Detalle de pago")
                    .FontSize(14)
                    .Bold()
                    .FontColor("#17202A");

                column.Item()
                    .PaddingTop(10)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn();
                        });

                        PaymentRow(
                            table,
                            "Interés bonificable",
                            _statement.BonusInterest);

                        PaymentRow(
                            table,
                            "Cuota mínima",
                            _statement.MinimumPayment);

                        PaymentRow(
                            table,
                            "Monto total a pagar",
                            _statement.TotalPayment);

                        PaymentRow(
                            table,
                            "Pago de contado con intereses",
                            _statement.CashPaymentWithInterest,
                            true);
                    });
            });
        }

        private static void PaymentRow(
            TableDescriptor table,
            string label,
            decimal amount,
            bool highlight = false)
                {
            var background = highlight
                ? "#FDECEF"
                : "#FFFFFF";

            table.Cell()
                .Background(background)
                .BorderBottom(1)
                .BorderColor("#E4E7EC")
                .Padding(10)
                .Text(label)
                .Bold();

            table.Cell()
                .Background(background)
                .BorderBottom(1)
                .BorderColor("#E4E7EC")
                .Padding(10)
                .AlignRight()
                .Text(amount.ToString("C2"))
                .Bold()
                .FontColor(
                    highlight
                        ? "#C8102E"
                        : "#17202A");
        }

        private void ComposeTransactions(IContainer container)
        {
            container.Column(column =>
            {
                column.Item()
                    .Text("Movimientos del mes")
                    .FontSize(14)
                    .Bold()
                    .FontColor("#17202A");

                column.Item()
                    .PaddingTop(10)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(95);
                            columns.ConstantColumn(70);
                            columns.RelativeColumn();
                            columns.ConstantColumn(85);
                        });

                        table.Header(header =>
                        {
                            HeaderCell(header, "Fecha");
                            HeaderCell(header, "Tipo");
                            HeaderCell(header, "Descripción");
                            HeaderCell(header, "Monto");
                        });

                        foreach (var transaction in _transactions)
                        {
                            var type =
                                transaction.TransactionType == "Purchase"
                                    ? "Compra"
                                    : transaction.TransactionType == "Payment"
                                        ? "Pago"
                                        : transaction.TransactionType;

                            BodyCell(
                                table,
                                transaction.TransactionDate
                                    .ToString("dd/MM/yyyy HH:mm"));

                            BodyCell(
                                table,
                                type);

                            BodyCell(
                                table,
                                transaction.Description);

                            table.Cell()
                                .BorderBottom(1)
                                .BorderColor("#EAECF0")
                                .PaddingVertical(8)
                                .PaddingHorizontal(6)
                                .AlignRight()
                                .Text(transaction.Amount.ToString("C2"))
                                .Bold();
                        }
                    });
            });
        }

        private static void HeaderCell(
            TableCellDescriptor header,
            string value)
        {
            header.Cell()
                .Background("#F2F4F7")
                .Padding(8)
                .Text(value)
                .Bold()
                .FontSize(9)
                .FontColor("#344054");
        }

        private static void BodyCell(
            TableDescriptor table,
            string value)
        {
            table.Cell()
                .BorderBottom(1)
                .BorderColor("#EAECF0")
                .PaddingVertical(8)
                .PaddingHorizontal(6)
                .Text(value)
                .FontSize(9);
        }
    }
}