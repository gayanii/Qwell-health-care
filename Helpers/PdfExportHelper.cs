using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.Win32;
using QWellApp.DBConnection;
using QWellApp.Models;
using QWellApp.ViewModels;
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

        // medical,procedure,lab,channel records pdf ('Download full summary' button click)
        public static void ExportFullReport(BaseSummaryViewModel summaryViewModel)
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
                        $"Full Report - (From {summaryViewModel.StartDateTime:dd-MM-yyyy HH:mm} to {summaryViewModel.EndDateTime:dd-MM-yyyy HH:mm})"
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

        // medical,procedure,lab,channel table templates for 'Download full summary' button click
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

        // Table 2 template
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

        // Individual summary report ('download medical summary', 'download lab summary' button clicks)
        public static void ExportReport(
            BaseSummaryViewModel summaryViewModel,
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
                    $"{reportTitle} (From {summaryViewModel.StartDateTime:dd-MM-yyyy HH:mm} to {summaryViewModel.EndDateTime:dd-MM-yyyy HH:mm})");

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

        // Making each report a pdf
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

        // Table 1 template
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


        // Commissions download

        public static void ExportToPDF(string filePath, string pageName, CommissionViewModel commissionViewModel)
        {
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    iText.Layout.Document document = new iText.Layout.Document(pdf);

                    // Add a topic/title to the PDF
                    iText.Layout.Element.Paragraph title = new iText.Layout.Element.Paragraph($"{pageName} Distribution Report - (From {commissionViewModel.StartDateTime:dd-MM-yyyy HH:mm} to {commissionViewModel.EndDateTime:dd-MM-yyyy HH:mm})")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Center the title
                    .SetFontSize(20) // Set font size for the title
                        .SetBold(); // Make the title bold

                    document.Add(title); // Add the title to the document

                    // Add some spacing between the title and tables
                    document.Add(new iText.Layout.Element.Paragraph("\n"));

                    // Create a table with the same number of columns as the DataGrid
                    iText.Layout.Element.Table table1 = new iText.Layout.Element.Table(6);

                    // Add header row with bold text
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("First Name").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Last Name").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Role").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Total Commission").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Dates").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Chit Number: Commission").SetBold()));

                    // Iterate over commissions to create rows with simulated rowspan
                    foreach (var commission in commissionViewModel.CommissionList)
                    {
                        // Get the number of rows to span
                        int rowCount = Math.Max(commission.Date.Count, commission.ChitNumber.Count);

                        // Add cells for the first row
                        table1.AddCell(new iText.Layout.Element.Cell(rowCount, 1).Add(new iText.Layout.Element.Paragraph(commission.FirstName.ToString())));
                        table1.AddCell(new iText.Layout.Element.Cell(rowCount, 1).Add(new iText.Layout.Element.Paragraph(commission.LastName.ToString())));

                        // Build the Role string if needed
                        string roles = string.Join(", ", commission.Role);
                        table1.AddCell(new iText.Layout.Element.Cell(rowCount, 1).Add(new iText.Layout.Element.Paragraph(roles)));

                        // Add total commission cell with rowspan
                        table1.AddCell(new iText.Layout.Element.Cell(rowCount, 1).Add(new iText.Layout.Element.Paragraph(commission.TotalCommisssion.ToString())));

                        // Add date and chit number cells for each row individually
                        for (int i = 0; i < rowCount; i++)
                        {
                            // Add date cell
                            if (i < commission.Date.Count)
                            {
                                table1.AddCell(commission.Date[i].ToString());
                            }
                            else
                            {
                                // Empty cell if no more dates are available
                                table1.AddCell("");
                            }

                            // Add chit number cell
                            if (i < commission.ChitNumber.Count)
                            {
                                table1.AddCell(commission.ChitNumber[i].ToString());
                            }
                            else
                            {
                                // Empty cell if no more chit numbers are available
                                table1.AddCell("");
                            }
                        }
                    }

                    // Add the table to the document
                    document.Add(table1);
                    document.Close();
                }
            }
        }
    }
}
