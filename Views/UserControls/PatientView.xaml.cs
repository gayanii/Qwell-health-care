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

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for PatientView.xaml
    /// </summary>
    public partial class PatientView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        public bool refresh;
        private IPatientRepository patientRepository;
        private ViewModelBase CurrentChildView;
        private PatientViewModel patientViewModel;

        public PatientView()
        {
            InitializeComponent();
            patientViewModel = new PatientViewModel();
            DataContext = patientViewModel;
            patientRepository = new PatientRepository();
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
            ((PatientViewModel)DataContext).LoadSearchResults.Execute(null);
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (QWellApp.Models.PatientView)dataGrid.SelectedItem;
                ((PatientViewModel)DataContext).SelectedId = selectedRowData.Id;
                selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void UpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                Patient patient = patientRepository.GetByID(selectedItemId);
                if (patient != null)
                {
                    PatientViewModel viewModel = new PatientViewModel();
                    viewModel.UpdateGridVisibility = true;
                    viewModel.PatientListVisibility = false;
                    viewModel.CreateGridVisibility = false;
                    viewModel.ResetUpdateButtonsVisibility = true;
                    DataContext = viewModel;
                    ((PatientViewModel)DataContext).SelectedId = selectedItemId;

                    FirstName.IsEnabled = true;
                    LastName.IsEnabled = true;
                    MobileNum.IsEnabled = true;
                    Telephone.IsEnabled = true;
                    Age.IsEnabled = true;
                    AllergicHistory.IsEnabled = true;
                    Weight.IsEnabled = true;
                    NIC.IsEnabled = true;
                    Status.IsEnabled = true;
                    Male.IsEnabled = true;
                    Female.IsEnabled = true;
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
                Patient patient = patientRepository.GetByID(selectedItemId);
                if (patient != null)
                {
                    deleteConfirmationPopup.IsOpen = true;
                    DeletePopupText.Text = "Do you want to delete " + patient.FirstName + "?";
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
            PatientViewModel viewModel = new PatientViewModel();
            viewModel.UpdateGridVisibility = false;
            viewModel.PatientListVisibility = false;
            viewModel.CreateGridVisibility = true;
            DataContext = viewModel;
        }

        private void ViewDetails(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            var selectedRowData = (QWellApp.Models.PatientView)dataGrid.SelectedItem;

            if (selectedRowData != null)
            {
                PatientViewModel viewModel = new PatientViewModel();
                viewModel.UpdateGridVisibility = true;
                viewModel.PatientListVisibility = false;
                viewModel.CreateGridVisibility = false;
                viewModel.ResetUpdateButtonsVisibility = false;
                DataContext = viewModel;

                ((PatientViewModel)DataContext).SelectedId = selectedRowData.Id;
                ((PatientViewModel)DataContext).GetPatientDetails.Execute(null);

                FirstName.IsEnabled = false;
                LastName.IsEnabled = false;
                MobileNum.IsEnabled = false;
                Telephone.IsEnabled = false;
                Age.IsEnabled = false;
                AllergicHistory.IsEnabled = false;
                Weight.IsEnabled = false;
                NIC.IsEnabled = false;
                Status.IsEnabled = false;
                Male.IsEnabled = false;
                Female.IsEnabled = false;
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
