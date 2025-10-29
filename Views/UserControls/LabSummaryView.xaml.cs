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
using QWellApp.Helpers;

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SummaryView.xaml
    /// </summary>
    public partial class LabSummaryView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        private ISummaryRepository summaryRepository;
        private ViewModelBase CurrentChildView;
        private LabSummaryViewModel summaryViewModel;

        private readonly List<string> labSummarytableHeaders = new List<string> { "ID", "Chit Number", "Admit Date", "Lab Bill", "Lab Paid Cost", "Consultation Fee", "Consumable Charges", "Total Commissions", "Total Bill"};
        
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

        public LabSummaryView()
        {
            InitializeComponent();
            summaryViewModel = new LabSummaryViewModel();
            DataContext = summaryViewModel;
            summaryRepository = new SummaryRepository();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void DownloadLabReportButton_Click(object sender, RoutedEventArgs e)
        {
            PdfExportHelper.ExportReport(
                summaryViewModel,
                "Lab Report",
                "QWell-Lab-Summary",
                labSummarytableHeaders,
                summaryViewModel.LabSummaryList.Cast<object>(),
                extractLabRowData,
                summaryViewModel.LabReportSummary);
        }

        private void DownloadFullReportButton_Click(object sender, RoutedEventArgs e)
        {
            PdfExportHelper.ExportFullReport(summaryViewModel);
        }
    }
}
