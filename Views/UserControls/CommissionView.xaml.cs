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
using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;
using System.ComponentModel.DataAnnotations;
using QWellApp.Helpers;

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
        private readonly Helpers.Validation validator = new Helpers.Validation();
        private int _previousEndValue;
        private int _previousStartValue;

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
                    PdfExportHelper.ExportToPDF(saveFileDialog.FileName, pageName, commissionViewModel);
                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9\.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void StartTimeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(DataContext is CommissionViewModel vm)) return;

            if (CommissionStartTime.SelectedValue is int newValue)
            {
                var currentVmValue = vm.StartTime;

                vm.StartTime = newValue;

                if (!IsVmDateRangeValid(vm))
                {
                    vm.StartTime = _previousStartValue;

                    CommissionStartTime.SelectedValue = _previousStartValue;
                }
                else
                {
                    _previousStartValue = newValue;
                }
            }
        }

        private void EndTimeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(DataContext is CommissionViewModel vm)) return;
            if (CommissionEndTime.SelectedValue is int newValue)
            {
                var currentVmValue = vm.EndTime;

                vm.EndTime = newValue;

                if (!IsVmDateRangeValid(vm))
                {
                    vm.EndTime = _previousEndValue;

                    CommissionEndTime.SelectedValue = _previousEndValue;
                }
                else
                {
                    _previousEndValue = newValue;
                }
            }
        }

        private bool IsVmDateRangeValid(CommissionViewModel vm)
        {
            var start = validator.CalculateStartDateTime(vm.StartDate, vm.StartTime);
            var end = validator.CalculateEndDateTime(vm.EndDate, vm.EndTime);
            return start <= end;
        }
    }
}
