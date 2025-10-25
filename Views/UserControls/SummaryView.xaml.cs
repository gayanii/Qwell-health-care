using iText.Kernel.Pdf;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using QWellApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SummaryView.xaml
    /// </summary>
    public partial class SummaryView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        private ISummaryRepository summaryRepository;
        private ViewModelBase CurrentChildView;
        private SummaryViewModel summaryViewModel;

        private readonly List<string> LabSummarytableHeaders = new List<string> { "ID", "Chit Number", "Admit Date", "Lab Bill", "Lab Paid Cost", "Consultation Fee", "Consumable Charges", "Total Commissions", "Total Bill"};
        private readonly List<string> ProcedureSummarytableHeaders = new List<string> { "ID", "Chit Number", "Admit Date", "OPD Charge", "Procedure Bill", "Consultation Fee", "Other Charges", "Total Commissions", "Total Bill" };
        private readonly List<string> MedicalSummarytableHeaders = new List<string> { "ID", "Chit Number", "Admit Date", "OPD Charge", "Pharmacy Bill", "Consultation Fee", "Other Charges", "Total Commissions", "Total Bill" };
        private readonly List<string> ChannelSummarytableHeaders = new List<string> { "ID", "Chit Number", "Admit Date", "OPD Charge", "Pharmacy Bill", "Consultation Fee", "Other Charges", "Total Commissions", "Total Bill" };

        Func<dynamic, List<string>> extractLabRowData = summary => new List<string>
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

        Func<dynamic, List<string>> extractMedicalRowData = summary => new List<string>
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

        Func<dynamic, List<string>> extractProcedureRowData = summary => new List<string>
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

        Func<dynamic, List<string>> extractChannelRowData = summary => new List<string>
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

        public SummaryView()
        {
            InitializeComponent();
            summaryViewModel = new SummaryViewModel();
            DataContext = summaryViewModel;
            summaryRepository = new SummaryRepository();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void DownloadMedicalReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (summaryViewModel.MedicalSummaryList == null || summaryViewModel.MedicalSummaryList.Count() == 0)
            {
                MessageBox.Show("No summary available to export.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"QWell-Medical-Summary-{summaryViewModel.StartDate.ToString("dd-MM-yyyy")}-to-{summaryViewModel.EndDate.ToString("dd-MM-yyyy")}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Generate document using ExportToPDF
                    var document = ExportToPDF(
                    saveFileDialog.FileName,
                    $"Medical Report - (From {summaryViewModel.StartDate:dd-MM-yyyy} 7.00AM to {summaryViewModel.EndDate.AddDays(1):dd-MM-yyyy} 6.59AM)");

                    var numberofRecords = new iText.Layout.Element.Paragraph($"Number of Patients: {summaryViewModel.MedicalSummaryList.Count()}")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) // Align text left
                       .SetFontSize(14)
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(numberofRecords);

                    // Add Table1 (Detailed Data Table)
                    var table1 = Table1(MedicalSummarytableHeaders, summaryViewModel.MedicalSummaryList, extractMedicalRowData, new iText.Layout.Element.Table(MedicalSummarytableHeaders.Count));
                    document.Add(table1.SetMarginBottom(10));

                    var spacerText = new iText.Layout.Element.Paragraph("Total Summary")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Align text center
                       .SetFontSize(14)
                       .SetBold()
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(spacerText); // Add the text to the document

                    // Add Table2 (Summary Table)
                    var table2 = Table2(summaryViewModel.MedicalReportSummary, new iText.Layout.Element.Table(4));
                    document.Add(table2);

                    // Close the document after adding all content
                    document.Close();

                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void DownloadProcedureReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (summaryViewModel.ProcedureSummaryList == null || summaryViewModel.ProcedureSummaryList.Count() == 0)
            {
                MessageBox.Show("No summary available to export.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"QWell-Procedure-Summary-{summaryViewModel.StartDate.ToString("dd-MM-yyyy")}-to-{summaryViewModel.EndDate.ToString("dd-MM-yyyy")}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Generate document using ExportToPDF
                    var document = ExportToPDF(
                    saveFileDialog.FileName,
                    $"Procedure Report - (From {summaryViewModel.StartDate:dd-MM-yyyy} 7.00AM to {summaryViewModel.EndDate.AddDays(1):dd-MM-yyyy} 6.59AM)");

                    var numberofRecords = new iText.Layout.Element.Paragraph($"Number of Patients: {summaryViewModel.ProcedureSummaryList.Count()}")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) // Align text left
                       .SetFontSize(14)
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(numberofRecords);

                    // Add Table1 (Detailed Data Table)
                    var table1 = Table1(ProcedureSummarytableHeaders, summaryViewModel.ProcedureSummaryList, extractProcedureRowData, new iText.Layout.Element.Table(ProcedureSummarytableHeaders.Count));
                    document.Add(table1.SetMarginBottom(10));

                    var spacerText = new iText.Layout.Element.Paragraph("Total Summary")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Align text center
                       .SetFontSize(14)
                       .SetBold()
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(spacerText); // Add the text to the document

                    // Add Table2 (Summary Table)
                    var table2 = Table2(summaryViewModel.ProcedureReportSummary, new iText.Layout.Element.Table(4));
                    document.Add(table2);

                    // Close the document after adding all content
                    document.Close(); 

                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void DownloadLabReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (summaryViewModel.LabSummaryList == null || summaryViewModel.LabSummaryList.Count() == 0)
            {
                MessageBox.Show("No summary available to export.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"QWell-Lab-Summary-{summaryViewModel.StartDate.ToString("dd-MM-yyyy")}-to-{summaryViewModel.EndDate.ToString("dd-MM-yyyy")}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Generate document using ExportToPDF
                    var document = ExportToPDF(
                        saveFileDialog.FileName,
                        $"Lab Report - (From {summaryViewModel.StartDate:dd-MM-yyyy} 7.00AM to {summaryViewModel.EndDate.AddDays(1):dd-MM-yyyy} 6.59AM)" );

                    var numberofRecords = new iText.Layout.Element.Paragraph($"Number of Patients: {summaryViewModel.LabSummaryList.Count()}")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) // Align text left
                       .SetFontSize(14)
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(numberofRecords);

                    // Add Table1 (Detailed Data Table)
                    var table1 = Table1(LabSummarytableHeaders, summaryViewModel.LabSummaryList, extractLabRowData, new iText.Layout.Element.Table(LabSummarytableHeaders.Count));
                    document.Add(table1.SetMarginBottom(10));

                    var spacerText = new iText.Layout.Element.Paragraph("Total Summary")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Align text center
                       .SetFontSize(14)
                       .SetBold()
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(spacerText); // Add the text to the document

                    // Add Table2 (Summary Table)
                    var table2 = Table2(summaryViewModel.LabReportSummary, new iText.Layout.Element.Table(4));
                    document.Add(table2);

                    // Close the document after adding all content
                    document.Close();

                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void DownloadChannelReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (summaryViewModel.ChannelSummaryList == null || summaryViewModel.ChannelSummaryList.Count() == 0)
            {
                MessageBox.Show("No summary available to export.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"QWell-Channel-Summary-{summaryViewModel.StartDate.ToString("dd-MM-yyyy")}-to-{summaryViewModel.EndDate.ToString("dd-MM-yyyy")}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Generate document using ExportToPDF
                    var document = ExportToPDF(
                    saveFileDialog.FileName,
                    $"Channel Report - (From {summaryViewModel.StartDate:dd-MM-yyyy} 7.00AM to {summaryViewModel.EndDate.AddDays(1):dd-MM-yyyy} 6.59AM)");

                    var numberofRecords = new iText.Layout.Element.Paragraph($"Number of Patients: {summaryViewModel.MedicalSummaryList.Count()}")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) // Align text left
                       .SetFontSize(14)
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(numberofRecords);

                    // Add Table1 (Detailed Data Table)
                    var table1 = Table1(ChannelSummarytableHeaders, summaryViewModel.MedicalSummaryList, extractChannelRowData, new iText.Layout.Element.Table(ChannelSummarytableHeaders.Count));
                    document.Add(table1.SetMarginBottom(10));

                    var spacerText = new iText.Layout.Element.Paragraph("Total Summary")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Align text center
                       .SetFontSize(14)
                       .SetBold()
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(spacerText); // Add the text to the document

                    // Add Table2 (Summary Table)
                    var table2 = Table2(summaryViewModel.ChannelReportSummary, new iText.Layout.Element.Table(4));
                    document.Add(table2);

                    // Close the document after adding all content
                    document.Close();

                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void DownloadFullReportButton_Click(object sender, RoutedEventArgs e)
        {
            if ((summaryViewModel.MedicalSummaryList == null || summaryViewModel.MedicalSummaryList.Count() == 0) &&
                (summaryViewModel.ProcedureSummaryList == null || summaryViewModel.ProcedureSummaryList.Count() == 0) && 
                (summaryViewModel.LabSummaryList == null || summaryViewModel.LabSummaryList.Count() == 0) &&
                (summaryViewModel.ChannelSummaryList == null || summaryViewModel.ChannelSummaryList.Count() == 0))
            {
                MessageBox.Show("No summary available to export.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"QWell-Full-Summary-{summaryViewModel.StartDate.ToString("dd-MM-yyyy")}-to-{summaryViewModel.EndDate.ToString("dd-MM-yyyy")}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Generate document using ExportToPDF
                    var document = ExportToPDF(
                        saveFileDialog.FileName,
                        $"Full Report - (From {summaryViewModel.StartDate:dd-MM-yyyy} 7.00AM to {summaryViewModel.EndDate.AddDays(1):dd-MM-yyyy} 6.59AM)");

                    var spacerText1 = new iText.Layout.Element.Paragraph("Summary of the medical records")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Align text center
                       .SetFontSize(14)
                       .SetBold()
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(spacerText1); // Add the text to the document

                    var numberofMedRecords = new iText.Layout.Element.Paragraph($"Number of Patients: {summaryViewModel.MedicalSummaryList.Count()}")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) // Align text left
                       .SetFontSize(14)
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(numberofMedRecords);

                    // Add Table1 (Medical Detailed Data Table)
                    var table1 = Table1(MedicalSummarytableHeaders, summaryViewModel.MedicalSummaryList, extractMedicalRowData, new iText.Layout.Element.Table(MedicalSummarytableHeaders.Count));
                    document.Add(table1.SetMarginBottom(10));

                    var spacerText2 = new iText.Layout.Element.Paragraph("Summary of the procedure records")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Align text center
                        .SetFontSize(14)
                        .SetBold()
                        .SetMarginTop(10)
                        .SetMarginBottom(10);

                    document.Add(spacerText2); // Add the text to the document

                    var numberofProRecords = new iText.Layout.Element.Paragraph($"Number of Patients: {summaryViewModel.ProcedureSummaryList.Count()}")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) // Align text left
                       .SetFontSize(14)
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(numberofProRecords);

                    // Add Table2 (Procedure Detailed Data Table)
                    var table2 = Table1(ProcedureSummarytableHeaders, summaryViewModel.ProcedureSummaryList, extractProcedureRowData, new iText.Layout.Element.Table(ProcedureSummarytableHeaders.Count));
                    document.Add(table2.SetMarginBottom(10));

                    var spacerText3 = new iText.Layout.Element.Paragraph("Summary of the lab records")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Align text center
                       .SetFontSize(14)
                       .SetBold()
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(spacerText3); // Add the text to the document

                    var numberofLabRecords = new iText.Layout.Element.Paragraph($"Number of Patients: {summaryViewModel.LabSummaryList.Count()}")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) // Align text left
                       .SetFontSize(14)
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(numberofLabRecords);

                    // Add Table3 (Lab Detailed Data Table)
                    var table3 = Table1(LabSummarytableHeaders, summaryViewModel.LabSummaryList, extractLabRowData, new iText.Layout.Element.Table(LabSummarytableHeaders.Count));
                    document.Add(table3.SetMarginBottom(10));

                    // Add Table4 (Summary Table)
                    var table4 = Table2(summaryViewModel.FullReportSummary, new iText.Layout.Element.Table(4));
                    document.Add(table4);

                    var spacerText5 = new iText.Layout.Element.Paragraph("Summary of the channelling records")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Align text center
                       .SetFontSize(14)
                       .SetBold()
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(spacerText5); // Add the text to the document

                    var numberofChannelRecords = new iText.Layout.Element.Paragraph($"Number of Patients: {summaryViewModel.ChannelSummaryList.Count()}")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) // Align text left
                       .SetFontSize(14)
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(numberofChannelRecords);

                    // Add Table1 (Medical Detailed Data Table)
                    var table5 = Table1(ChannelSummarytableHeaders, summaryViewModel.ChannelSummaryList, extractChannelRowData, new iText.Layout.Element.Table(ChannelSummarytableHeaders.Count));
                    document.Add(table1.SetMarginBottom(10));

                    var spacerText = new iText.Layout.Element.Paragraph("Total Summary")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Align text center
                       .SetFontSize(14)
                       .SetBold()
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(spacerText); // Add the text to the document

                    var tot = summaryViewModel.MedicalSummaryList.Count() + summaryViewModel.ProcedureSummaryList.Count() + summaryViewModel.LabSummaryList.Count();
                    var numberofRecords = new iText.Layout.Element.Paragraph($"Total Number of Patients: {tot}")
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) // Align text left
                       .SetFontSize(14)
                       .SetMarginTop(10)
                       .SetMarginBottom(10);

                    document.Add(numberofRecords);

                    // Close the document after adding all content
                    document.Close();

                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private iText.Layout.Document ExportToPDF(string filePath,string reportTitle)
        {
            PdfWriter writer = new PdfWriter(filePath);
            PdfDocument pdf = new PdfDocument(writer);
            iText.Layout.Document document = new iText.Layout.Document(pdf);

            // Add a title to the PDF
            iText.Layout.Element.Paragraph title = new iText.Layout.Element.Paragraph(reportTitle)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(20)
                .SetBold();

            document.Add(title);
            document.Add(new iText.Layout.Element.Paragraph("\n"));

            return document;
        }

        private iText.Layout.Element.Table Table1<T>(
            List<string> tableHeaders,
            IEnumerable<T> summaryList,
            Func<T, List<string>> extractRowData,
            iText.Layout.Element.Table table1)
        {
            // Table for detailed data
            table1 = new iText.Layout.Element.Table(tableHeaders.Count);
            foreach (var header in tableHeaders)
            {
                var headerCell = new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(header).SetBold());
                table1.AddHeaderCell(headerCell);
            }

            // Add rows dynamically based on the data
            foreach (var summary in summaryList)
            {
                var rowData = extractRowData(summary);
                foreach (var cellData in rowData)
                {
                    table1.AddCell(cellData);
                }
            }

            return table1;
        }

        private iText.Layout.Element.Table Table2(Report reportData, iText.Layout.Element.Table table2)
        {
            // Summary Table
            table2 = new iText.Layout.Element.Table(4);
            table2.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Total Income").SetBold()));
            table2.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Total Lab Paid").SetBold()));
            table2.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Total Commissions").SetBold()));
            table2.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Balance (Total Income - Total Lab Paid - Total Commissions)").SetBold()));

            table2.AddCell(reportData.TotalIncome.ToString());
            table2.AddCell(reportData.TotalLabPaid.ToString());
            table2.AddCell(reportData.TotalCommissions.ToString());
            table2.AddCell(reportData.Balance.ToString());

            return table2;
        }

    }
}
