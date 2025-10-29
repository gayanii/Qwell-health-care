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
    public partial class MedicalSummaryView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        private ISummaryRepository summaryRepository;
        private ViewModelBase CurrentChildView;
        private MedicalSummaryViewModel summaryViewModel;

        private readonly List<string> medicalSummarytableHeaders = new List<string> { "ID", "Chit Number", "Admit Date", "OPD Charge", "Pharmacy Bill", "Consultation Fee", "Other Charges", "Total Commissions", "Total Bill" };
        
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

        public MedicalSummaryView()
        {
            InitializeComponent();
            summaryViewModel = new MedicalSummaryViewModel();
            DataContext = summaryViewModel;
            summaryRepository = new SummaryRepository();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void DownloadMedicalReportButton_Click(object sender, RoutedEventArgs e)
        {
            PdfExportHelper.ExportReport(
                summaryViewModel,
                "Medical Report",
                "QWell-Medical-Summary",
                medicalSummarytableHeaders,
                summaryViewModel.MedicalSummaryList.Cast<object>(),
                extractMedicalRowData,
                summaryViewModel.MedicalReportSummary);
        }

        private void DownloadFullReportButton_Click(object sender, RoutedEventArgs e)
        {
            PdfExportHelper.ExportFullReport(summaryViewModel);
        }

    }
}
