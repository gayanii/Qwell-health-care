using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using QWellApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using QWellApp.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Cryptography;
using Azure.Core.GeoJson;

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for PatientView.xaml
    /// </summary>
    public partial class LabRecordView : UserControl
    {
        private ViewModelBase CurrentChildView;
        private LabRecordViewModel labRecordViewModel;
        public int selectedItemId;
        private bool isItemSelected = false;
        public bool refresh;
        private IUserRepository userRepository;
        private ILabRecordRepository labRecordRepository;

        public LabRecordView()
        {
            InitializeComponent();
            labRecordViewModel = new LabRecordViewModel();
            DataContext = labRecordViewModel;
            userRepository = new UserRepository();
            labRecordRepository = new LabRecordRepository();
            // Attach the TextChanged event handler for search
            SearchBox.TextChanged += Search_TextChanged;
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Prevent users from manually typing a date
            e.Handled = true;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((LabRecordViewModel)DataContext).LoadSearchResults.Execute(null);
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (QWellApp.Models.LabRecordView)dataGrid.SelectedItem;
                ((LabRecordViewModel)DataContext).SelectedId = selectedRowData.Id;
                selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void UpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                LabRecord labRecord = labRecordRepository.GetByID(selectedItemId);
                if (labRecord != null)
                {
                    LabRecordViewModel viewModel = new LabRecordViewModel();
                    viewModel.UpdateGridVisibility = true;
                    viewModel.LabRecordListVisibility = false;
                    viewModel.CreateGridVisibility = false;
                    viewModel.ResetUpdateButtonsVisibility = true;
                    DataContext = viewModel;
                    ((LabRecordViewModel)DataContext).SelectedId = selectedItemId;

                    Patient.IsEnabled = true;
                    ChitNumber.IsEnabled = true;
                    AdmitDate.IsEnabled = false;
                    TotalBill.IsEnabled = false;
                    LabBill.IsEnabled = false;
                    OtherCharges.IsEnabled = false;
                    Doctor.IsEnabled = true;
                    Nurse1.IsEnabled = true;
                    Nurse2.IsEnabled = true;
                    DocComm.IsEnabled = true;
                    Nurse1Comm.IsEnabled = true;
                    Nurse2Comm.IsEnabled = true;

                    //Lab fields
                    var labArray = new[] { Lab1, Lab2, Lab3, Lab4, Lab5, Lab6, Lab7, Lab8, Lab9, Lab10 };
                    var labTotalArray = new[] { LabTotal1, LabTotal2, LabTotal3, LabTotal4, LabTotal5, LabTotal6, LabTotal7, LabTotal8, LabTotal9, LabTotal10 };
                    var totalArray = new[] { Total1, Total2, Total3, Total4, Total5, Total6, Total7, Total8, Total9, Total10 };
                    var medArray = new[] { Med1, Med2, Med3, Med4, Med5, Med6, Med7, Med8, Med9, Med10};
                    var doseArray = new[] { Dose1, Dose2, Dose3, Dose4, Dose5, Dose6, Dose7, Dose8, Dose9, Dose10 };
                    foreach (var lab in labArray)
                    {
                        lab.IsEnabled = true;
                    }
                    foreach (var total in labTotalArray)
                    {
                        total.IsEnabled = false;
                    }
                    foreach (var total in totalArray)
                    {
                        total.IsEnabled = false;
                    }
                    foreach (var med in medArray)
                    {
                        med.IsEnabled = true;
                    }
                    foreach (var day in doseArray)
                    {
                        day.IsEnabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select an item first!");
            }
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                LabRecord labRecord = labRecordRepository.GetByID(selectedItemId);
                if (labRecord != null)
                {
                    deleteConfirmationPopup.IsOpen = true;
                    DeletePopupText.Text = "Do you want to delete this lab record ID " + labRecord.Id + "?";
                }
                else
                {
                    MessageBox.Show("Select an item first!");
                }
            }
            else
            {
                MessageBox.Show("Select an item first!");
            }
        }

        private void CloseDeleteConfirmationPopup()
        {
            deleteConfirmationPopup.IsOpen = false;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDeleteConfirmationPopup();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDeleteConfirmationPopup();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Close the popup when clicked outside of it
            if (deleteConfirmationPopup.IsOpen && !deleteConfirmationPopup.Child.IsMouseOver)
            {
                CloseDeleteConfirmationPopup();
            }
        }

        private void CreateButtonClicked(object sender, RoutedEventArgs e)
        {
            LabRecordViewModel viewModel = new LabRecordViewModel();
            viewModel.UpdateGridVisibility = false;
            viewModel.LabRecordListVisibility = false;
            viewModel.CreateGridVisibility = true;
            TotalBillCreate.IsEnabled = false;
            DataContext = viewModel;
            AdmitDateCreate.IsEnabled = false;
            LabBillCreate.IsEnabled = false;
            OtherChargesCreate.IsEnabled = false;

            var labTotalArray = new[] { LabTotal1Create, LabTotal2Create, LabTotal3Create, LabTotal4Create, LabTotal5Create, LabTotal6Create, LabTotal7Create, LabTotal8Create, LabTotal9Create, LabTotal10Create };
            var totalArray = new[] { Total1Create, Total2Create, Total3Create, Total4Create, Total5Create, Total6Create, Total7Create, Total8Create, Total9Create, Total10Create };
            foreach (var total in totalArray)
            {
                total.IsEnabled = false;
            }
            foreach (var total in labTotalArray)
            {
                total.IsEnabled = false;
            }
        }

        private void ViewDetails(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            var selectedRowData = (QWellApp.Models.LabRecordView)dataGrid.SelectedItem;

            if (selectedRowData != null)
            {
                LabRecordViewModel viewModel = new LabRecordViewModel();
                viewModel.UpdateGridVisibility = true;
                viewModel.LabRecordListVisibility = false;
                viewModel.CreateGridVisibility = false;
                viewModel.ResetUpdateButtonsVisibility = false;
                DataContext = viewModel;

                ((LabRecordViewModel)DataContext).SelectedId = selectedRowData.Id;
                ((LabRecordViewModel)DataContext).GetLabRecordDetails.Execute(null);

                Patient.IsEnabled = false;
                ChitNumber.IsEnabled = false;
                AdmitDate.IsEnabled = false;
                TotalBill.IsEnabled = false;
                OtherCharges.IsEnabled = false;
                ConsultantFee.IsEnabled = false;
                LabBill.IsEnabled = false;
                Doctor.IsEnabled = false;
                DocComm.IsEnabled = false;
                Nurse1Comm.IsEnabled = false;
                Nurse2Comm.IsEnabled = false;
                Nurse1.IsEnabled = false;
                Nurse2.IsEnabled = false;

                //Lab fields
                var labArray = new[] { Lab1, Lab2, Lab3, Lab4, Lab5, Lab6, Lab7, Lab8, Lab9, Lab10 };
                var labTotalArray = new[] { LabTotal1, LabTotal2, LabTotal3, LabTotal4, LabTotal5, LabTotal6, LabTotal7, LabTotal8, LabTotal9, LabTotal10 };
                var totalArray = new[] { Total1, Total2, Total3, Total4, Total5, Total6, Total7, Total8, Total9, Total10 };
                var medArray = new[] { Med1, Med2, Med3, Med4, Med5, Med6, Med7, Med8, Med9, Med10};
                var doseArray = new[] { Dose1, Dose2, Dose3, Dose4, Dose5, Dose6, Dose7, Dose8, Dose9, Dose10 };
                foreach (var lab in labArray)
                {
                    lab.IsEnabled = false;
                }
                foreach (var total in labTotalArray)
                {
                    total.IsEnabled = false;
                }
                foreach (var total in totalArray)
                {
                    total.IsEnabled = false;
                }
                foreach (var med in medArray)
                {
                    med.IsEnabled = false;
                }
                foreach (var day in doseArray)
                {
                    day.IsEnabled = false;
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
