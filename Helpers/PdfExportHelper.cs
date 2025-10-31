using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.Win32;
using QWellApp.DBConnection;
using QWellApp.Models;
using QWellApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QWellApp.Helpers
{
    public class PdfExportHelper
    {
        private static readonly List<string> LabSummaryHeaders = new List<string>
        {
            "ID", "Chit Number", "Admit Date", "Lab Bill", "Lab Paid Cost",
            "Consultation Fee", "Consumable Charges", "Total Commissions", "Total Bill"
        };

        private static readonly List<string> ProcedureSummaryHeaders = new List<string>
        {
            "ID", "Chit Number", "Admit Date", "OPD Charge", "Procedure Bill",
            "Consultation Fee", "Other Charges", "Total Commissions", "Total Bill"
        };

        private static readonly List<string> MedicalSummaryHeaders = new List<string>
        {
            "ID", "Chit Number", "Admit Date", "OPD Charge", "Pharmacy Bill",
            "Consultation Fee", "Other Charges", "Total Commissions", "Total Bill"
        };

        private static readonly List<string> ChannelSummaryHeaders = new List<string>
        {
            "ID", "Chit Number", "Admit Date", "OPD Charge", "Pharmacy Bill",
            "Consultation Fee", "Other Charges", "Total Commissions", "Total Bill"
        };

        // Row data extractors
        private static readonly Func<dynamic, List<string>> extractLabRowData = summary => new List<string>
        {
            summary.Id.ToString(),
            summary.ChitNumber,
            summary.AdmitDate.ToString(),
            summary.LabBill.ToString(),
            summary.LabPaidCost.ToString(),
            summary.ConsultantFee.ToString(),
            summary.ConsumableBill.ToString(),
            summary.TotalCommisions.ToString(),
            summary.TotalBill.ToString()
        };

        private static readonly Func<dynamic, List<string>> extractMedicalRowData = summary => new List<string>
        {
            summary.Id.ToString(),
            summary.ChitNumber,
            summary.AdmitDate.ToString(),
            summary.OPDCharge.ToString(),
            summary.PharmacyBill.ToString(),
            summary.ConsultantFee.ToString(),
            summary.OtherCharges.ToString(),
            summary.TotalCommisions.ToString(),
            summary.TotalBill.ToString()
        };

        private static readonly Func<dynamic, List<string>> extractProcedureRowData = summary => new List<string>
        {
            summary.Id.ToString(),
            summary.ChitNumber,
            summary.AdmitDate.ToString(),
            summary.OPDCharge.ToString(),
            summary.ProcedureBill.ToString(),
            summary.ConsultantFee.ToString(),
            summary.OtherCharges.ToString(),
            summary.TotalCommisions.ToString(),
            summary.TotalBill.ToString()
        };

        private static readonly Func<dynamic, List<string>> extractChannelRowData = summary => new List<string>
        {
            summary.Id.ToString(),
            summary.ChitNumber,
            summary.AdmitDate.ToString(),
            summary.OPDCharge.ToString(),
            summary.PharmacyBill.ToString(),
            summary.ConsultantFee.ToString(),
            summary.OtherCharges.ToString(),
            summary.TotalCommisions.ToString(),
            summary.TotalBill.ToString()
        };

        public static void ExportFullReport(SummaryViewModel summaryViewModel)
        {
            if ((summaryViewModel.MedicalSummaryList == null || !summaryViewModel.MedicalSummaryList.Any()) &&
                (summaryViewModel.ProcedureSummaryList == null || !summaryViewModel.ProcedureSummaryList.Any()) &&
                (summaryViewModel.LabSummaryList == null || !summaryViewModel.LabSummaryList.Any()) &&
                (summaryViewModel.ChannelSummaryList == null || !summaryViewModel.ChannelSummaryList.Any()))
            {
                MessageBox.Show("No summary available to export.");
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"QWell-Full-Summary-{summaryViewModel.StartDate:dd-MM-yyyy}-to-{summaryViewModel.EndDate:dd-MM-yyyy}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var document = ExportToPDF(
                        saveFileDialog.FileName,
                        $"Full Report - (From {summaryViewModel.StartDate:dd-MM-yyyy} 7.00AM to {summaryViewModel.EndDate.AddDays(1):dd-MM-yyyy} 6.59AM)"
                    );

                    AddSummarySection(document, "Summary of the medical records", summaryViewModel.MedicalSummaryList, MedicalSummaryHeaders, extractMedicalRowData, r => (float)r.TotalBill);
                    AddSummarySection(document, "Summary of the procedure records", summaryViewModel.ProcedureSummaryList, ProcedureSummaryHeaders, extractProcedureRowData, r => (float)r.TotalBill);
                    AddSummarySection(document, "Summary of the lab records", summaryViewModel.LabSummaryList, LabSummaryHeaders, extractLabRowData, r => (float)r.TotalBill);
                    AddSummarySection(document, "Summary of the channelling records", summaryViewModel.ChannelSummaryList, ChannelSummaryHeaders, extractChannelRowData, r => (float)r.TotalBill);

                    // Total summary
                    var total = summaryViewModel.MedicalSummaryList.Count()
                        + summaryViewModel.ProcedureSummaryList.Count()
                        + summaryViewModel.LabSummaryList.Count()
                        + summaryViewModel.ChannelSummaryList.Count();

                    document.Add(new Paragraph("Total Summary")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(14)
                        .SetBold()
                        .SetMarginTop(10)
                        .SetMarginBottom(10));

                    document.Add(new Paragraph($"Total Number of Patients: {total}")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .SetFontSize(14)
                        .SetMarginTop(10)
                        .SetMarginBottom(10));

                    document.Add(CreateSummaryTable(summaryViewModel.FullReportSummary));

                    document.Close();
                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private static void AddSummarySection<T>(
            iText.Layout.Document document,
            string title,
            IEnumerable<T> list,
            List<string> headers,
            Func<T, List<string>> extractRowData,
            Func<T, float>? extractTotal = null)
        {
            document.Add(new Paragraph(title)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(14)
                .SetBold()
                .SetMarginTop(10)
                .SetMarginBottom(10));

            document.Add(new Paragraph($"Number of Patients: {list.Count()}")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                .SetFontSize(14)
                .SetMarginTop(10)
                .SetMarginBottom(10));

            var table = new Table(headers.Count);
            foreach (var header in headers)
                table.AddHeaderCell(new Cell().Add(new Paragraph(header).SetBold()));

            foreach (var item in list)
            {
                var rowData = extractRowData(item);
                foreach (var cell in rowData)
                    table.AddCell(cell);
            }

            document.Add(table.SetMarginBottom(10));

            if (extractTotal != null)
            {
                var total = list.Sum(extractTotal);
                var totalText = new Paragraph($"Total: {Math.Round(total, 2):N2}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetMarginTop(10)
                    .SetMarginBottom(10);

                document.Add(totalText);
            }
        }

        private static Table CreateSummaryTable(Report reportData)
        {
            var table = new Table(4);
            table.AddHeaderCell(new Cell().Add(new Paragraph("Total Income").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Total Lab Paid").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Total Commissions").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Balance (Total Income - Total Lab Paid - Total Commissions)").SetBold()));

            table.AddCell($"{Math.Round(reportData.TotalIncome ?? 0, 2):N2}");
            table.AddCell($"{Math.Round(reportData.TotalLabPaid ?? 0, 2):N2}");
            table.AddCell($"{Math.Round(reportData.TotalCommissions ?? 0, 2):N2}");
            table.AddCell($"{Math.Round(reportData.Balance ?? 0, 2):N2}");

            return table;
        }

        public static void ExportReport(
            SummaryViewModel summaryViewModel,
            string reportTitle,
            string defaultFileName,
            List<string> tableHeaders,
            IEnumerable<object> summaryList,
            Func<object, List<string>> extractRowData,
            Report reportSummary)
        {
            if (summaryList == null || !summaryList.Any())
            {
                MessageBox.Show("No summary available to export.");
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"{defaultFileName}-{summaryViewModel.StartDate:dd-MM-yyyy}-to-{summaryViewModel.EndDate:dd-MM-yyyy}.pdf"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            try
            {
                var document = ExportToPDF(
                    saveFileDialog.FileName,
                    $"{reportTitle} (From {summaryViewModel.StartDate:dd-MM-yyyy} 7.00AM to {summaryViewModel.EndDate.AddDays(1):dd-MM-yyyy} 6.59AM)");

                var countParagraph = new Paragraph($"Number of Patients: {summaryList.Count()}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                    .SetFontSize(14)
                    .SetMarginTop(10)
                    .SetMarginBottom(10);

                document.Add(countParagraph);

                // Table 1 (Detailed Data)
                var table1 = Table1(tableHeaders, summaryList, extractRowData, new Table(tableHeaders.Count));
                document.Add(table1.SetMarginBottom(10));

                var totalText = new Paragraph($"Total: {Math.Round(reportSummary.TotalIncome ?? 0, 2):N2}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetMarginTop(10)
                    .SetMarginBottom(10);

                document.Add(totalText);

                var spacerText = new Paragraph("Total Summary")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetBold()
                    .SetMarginTop(10)
                    .SetMarginBottom(10);

                document.Add(spacerText);

                // Table 2 (Summary)
                var table2 = CreateSummaryTable(reportSummary);
                document.Add(table2);

                document.Close();
                MessageBox.Show("PDF exported successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private static iText.Layout.Document ExportToPDF(string fileName, string title)
        {
            PdfWriter writer = new PdfWriter(fileName);
            PdfDocument pdf = new PdfDocument(writer);
            iText.Layout.Document document = new iText.Layout.Document(pdf);

            // Add a title to the PDF
            Paragraph reportTitle = new Paragraph(title)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(20)
                .SetBold();

            document.Add(reportTitle);
            document.Add(new Paragraph("\n"));

            return document;
        }

        private static Table Table1(List<string> headers, IEnumerable<object> summaryList, Func<object, List<string>> extractRowData, Table table)
        {
            // Table for detailed data
            table = new Table(headers.Count);
            foreach (var header in headers)
            {
                var headerCell = new Cell().Add(new Paragraph(header).SetBold());
                table.AddHeaderCell(headerCell);
            }

            // Add rows dynamically based on the data
            foreach (var summary in summaryList)
            {
                var rowData = extractRowData(summary);
                foreach (var cellData in rowData)
                {
                    table.AddCell(cellData);
                }
            }

            return table;
        }
    }
}
