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
    public partial class StockView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        private ViewModelBase CurrentChildView;
        private StockViewModel stockViewModel;

        public StockView()
        {
            InitializeComponent();
            stockViewModel = new StockViewModel();
            DataContext = stockViewModel;
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (QWellApp.Models.Stock)dataGrid.SelectedItem;
                //((CommissionViewModel)DataContext).SelectedId = selectedRowData.Id;
                //selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void DownloadPDFButton_Click(object sender, RoutedEventArgs e)
        {
            if (stockViewModel.StockList == null || stockViewModel.StockList.Count() == 0)
            {
                System.Windows.MessageBox.Show("No stock data available to export.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"QWell-Stock-Report-{stockViewModel.StartDate.ToString("dd-MM-yyyy")}-to-{stockViewModel.EndDate.ToString("dd-MM-yyyy")}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    ExportToPDF(saveFileDialog.FileName);
                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void ExportToPDF(string filePath)
        {
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    iText.Layout.Document document = new iText.Layout.Document(pdf);

                    // Add a topic/title to the PDF
                    iText.Layout.Element.Paragraph title = new iText.Layout.Element.Paragraph($"Stock Report - (From {stockViewModel.StartDate.ToString("dd-MM-yyyy")} 7.00AM to {stockViewModel.EndDate.AddDays(1).ToString("dd-MM-yyyy")} 6.59AM)")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Center the title
                        .SetFontSize(20) // Set font size for the title
                        .SetBold(); // Make the title bold

                    document.Add(title); // Add the title to the document

                    // Add some spacing between the title and tables
                    document.Add(new iText.Layout.Element.Paragraph("\n"));

                    // Create a table with the same number of columns as the DataGrid
                    iText.Layout.Element.Table table1 = new iText.Layout.Element.Table(6)
                        .SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(100)); // Set table width to 100%;

                    // Add header row with bold styling
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Id").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Brand Name").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Generic").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Collected Stock").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Sold Stock").SetBold()));
                    table1.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Balance (Collected - Sold Stock)").SetBold()));

                    // Add data rows
                    foreach (var stock in stockViewModel.StockList)
                    {
                        table1.AddCell(stock.Id.ToString());
                        table1.AddCell(stock.BrandName.ToString());
                        table1.AddCell(stock.Generic.ToString());
                        table1.AddCell(stock.CollectedStock.ToString());
                        table1.AddCell(stock.SoldStock.ToString());
                        table1.AddCell(stock.Balance.ToString());
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
