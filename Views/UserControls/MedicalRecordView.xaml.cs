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
using QWellApp.Repositories;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MedicalRecordView.xaml
    /// </summary>
    public partial class MedicalRecordView : System.Windows.Controls.UserControl
    {
        private ViewModelBase CurrentChildView;
        private MedicalRecordViewModel medicalRecordViewModel;
        public int selectedItemId;
        private bool isItemSelected = false;
        public bool refresh;
        private IUserRepository userRepository;
        private IMedicalRecordRepository medicalRecordRepository;

        public MedicalRecordView()
        {
            InitializeComponent();
            medicalRecordViewModel = new MedicalRecordViewModel();
            DataContext = medicalRecordViewModel;
            userRepository = new UserRepository();
            medicalRecordRepository = new MedicalRecordRepository();
            // Attach the TextChanged event handler for search
            SearchBox.TextChanged += Search_TextChanged;
        }

        private void DatePicker_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Prevent users from manually typing a date
            e.Handled = true;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((MedicalRecordViewModel)DataContext).LoadSearchResults.Execute(null);
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (QWellApp.Models.MedicalRecordView)dataGrid.SelectedItem;
                ((MedicalRecordViewModel)DataContext).SelectedId = selectedRowData.Id;
                selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void UpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                MedicalRecord medicalRecord = medicalRecordRepository.GetByID(selectedItemId);
                if (medicalRecord != null)
                {
                    MedicalRecordViewModel viewModel = new MedicalRecordViewModel();
                    viewModel.UpdateGridVisibility = true;
                    viewModel.MedicalRecordListVisibility = false;
                    viewModel.CreateGridVisibility = false;
                    viewModel.ResetUpdateButtonsVisibility = true;
                    DataContext = viewModel;
                    ((MedicalRecordViewModel)DataContext).SelectedId = selectedItemId;

                    ChitNumber.IsEnabled = true;
                    OPDCharge.IsEnabled = true;
                    PharmacyBill.IsEnabled = false;
                    ConsultantFee.IsEnabled = true;
                    TotalBill.IsEnabled = false;
                    OtherCharges.IsEnabled = true;
                    AdmitDate.IsEnabled = false;
                    Patient.IsEnabled = true;
                    Doctor.IsEnabled = true;
                    DocComm.IsEnabled = true;
                    Nurse1Comm.IsEnabled = true;
                    Nurse2Comm.IsEnabled = true;
                    Nurse1.IsEnabled = true;
                    Nurse2.IsEnabled = true;

                    //Medicine fields
                    var medArray = new[] { Med1, Med2, Med3, Med4, Med5, Med6, Med7, Med8, Med9, Med10, Med11, Med12};
                    var doseArray = new[] { Dose1, Dose2, Dose3, Dose4, Dose5, Dose6, Dose7, Dose8, Dose9, Dose10, Dose11, Dose12 };
                    var totalArray = new[] { Total1, Total2, Total3, Total4, Total5, Total6, Total7, Total8, Total9, Total10, Total11, Total12 };

                    foreach (var med in medArray)
                    {
                        med.IsEnabled = true;
                    }

                    foreach (var day in doseArray)
                    {
                        day.IsEnabled = true;
                    }

                    foreach (var total in totalArray)
                    {
                        total.IsEnabled = false;
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Select an item first!");
            }
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                MedicalRecord medicalRecord = medicalRecordRepository.GetByID(selectedItemId);
                if (medicalRecord != null)
                {
                    deleteConfirmationPopup.IsOpen = true;
                    DeletePopupText.Text = "Do you want to delete this medical record ID " + medicalRecord.Id + "?";
                }
                else
                {
                    System.Windows.MessageBox.Show("Select an item first!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Select an item first!");
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
            MedicalRecordViewModel viewModel = new MedicalRecordViewModel();
            viewModel.UpdateGridVisibility = false;
            viewModel.MedicalRecordListVisibility = false;
            viewModel.CreateGridVisibility = true;
            PharmacyBillCreate.IsEnabled = false;
            TotalBillCreate.IsEnabled = false;
            AdmitDateCreate.IsEnabled = false;
            DataContext = viewModel;

            var totalArray = new[] { Total1Create, Total2Create, Total3Create, Total4Create, Total5Create, Total6Create, Total7Create, Total8Create, Total9Create, Total10Create, Total11Create, Total12Create };

            foreach (var total in totalArray)
            {
                total.IsEnabled = false;
            }
        }

        private void ViewDetails(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            var selectedRowData = (QWellApp.Models.MedicalRecordView)dataGrid.SelectedItem;

            if (selectedRowData != null)
            {
                MedicalRecordViewModel viewModel = new MedicalRecordViewModel();
                viewModel.UpdateGridVisibility = true;
                viewModel.MedicalRecordListVisibility = false;
                viewModel.CreateGridVisibility = false;
                viewModel.ResetUpdateButtonsVisibility = false;
                DataContext = viewModel;

                ((MedicalRecordViewModel)DataContext).SelectedId = selectedRowData.Id;
                ((MedicalRecordViewModel)DataContext).GetMedicalRecordDetails.Execute(null);

                ChitNumber.IsEnabled = false;
                OPDCharge.IsEnabled = false;
                PharmacyBill.IsEnabled = false;
                TotalBill.IsEnabled = false;
                OtherCharges.IsEnabled = false;
                ConsultantFee.IsEnabled = false;
                AdmitDate.IsEnabled = false;
                Patient.IsEnabled = false;
                Doctor.IsEnabled = false;
                DocComm.IsEnabled = false;
                Nurse1Comm.IsEnabled = false;
                Nurse2Comm.IsEnabled = false;
                Nurse1.IsEnabled = false;
                Nurse2.IsEnabled = false;

                //Medicine fields
                var medArray = new[] { Med1, Med2, Med3, Med4, Med5, Med6, Med7, Med8, Med9, Med10, Med11, Med12 };
                var doseArray = new[] { Dose1, Dose2, Dose3, Dose4, Dose5, Dose6, Dose7, Dose8, Dose9, Dose10, Dose11, Dose12 };
                var totalArray = new[] { Total1, Total2, Total3, Total4, Total5, Total6, Total7, Total8, Total9, Total10, Total11, Total12 };

                foreach (var med in medArray)
                {
                    med.IsEnabled = false;
                }

                foreach (var day in doseArray)
                {
                    day.IsEnabled = false;
                }

                foreach (var total in totalArray)
                {
                    total.IsEnabled = false;
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
