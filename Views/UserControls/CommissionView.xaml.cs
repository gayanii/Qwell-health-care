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
using iText.Layout.Element;
using Microsoft.Win32;
using System.Globalization;

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CommissionView.xaml
    /// </summary>
    public partial class CommissionView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        private ISupplierRepository supplierRepository;
        private ViewModelBase CurrentChildView;
        private CommissionViewModel commissionViewModel;

        public CommissionView()
        {
            InitializeComponent();
            commissionViewModel = new CommissionViewModel();
            DataContext = commissionViewModel;
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (QWellApp.Models.Commission)dataGrid.SelectedItem;
                //((CommissionViewModel)DataContext).SelectedId = selectedRowData.Id;
                //selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void DownloadPDFButton_Click(object sender, RoutedEventArgs e)
        {
            if (commissionViewModel.CommissionList == null || commissionViewModel.CommissionList.Count() == 0)
            {
                System.Windows.MessageBox.Show("No commissions available to export.");
                return;
            }

            string pageName = (string)System.Windows.Application.Current.Properties["PageName"];

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"QWell-{pageName}-Summary-{commissionViewModel.StartDate.ToString("dd-MM-yyyy")}-to-{commissionViewModel.EndDate.ToString("dd-MM-yyyy")}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    ExportToPDF(saveFileDialog.FileName, pageName);
                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void ExportToPDF(string filePath, string pageName)
        {
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    iText.Layout.Document document = new iText.Layout.Document(pdf);

                    // Add a topic/title to the PDF
                    iText.Layout.Element.Paragraph title = new iText.Layout.Element.Paragraph($"{pageName} Distribution Report - (From {commissionViewModel.StartDate.ToString("dd-MM-yyyy")} 7.00AM to {commissionViewModel.EndDate.AddDays(1).ToString("dd-MM-yyyy")} 6.59AM)")
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9\.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
