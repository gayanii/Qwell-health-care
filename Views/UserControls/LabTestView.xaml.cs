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
    public partial class LabTestView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        public bool refresh;
        private ILabTestRepository labTestRepository;
        private ViewModelBase CurrentChildView;
        private LabTestViewModel labTestViewModel;

        public LabTestView()
        {
            InitializeComponent();
            labTestViewModel = new LabTestViewModel();
            DataContext = labTestViewModel;
            labTestRepository = new LabTestRepository();
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
            ((LabTestViewModel)DataContext).LoadSearchResults.Execute(null);
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (QWellApp.Models.LabTestView)dataGrid.SelectedItem;
                ((LabTestViewModel)DataContext).SelectedId = selectedRowData.Id;
                selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void UpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                LabTest labTest = labTestRepository.GetByID(selectedItemId);
                if (labTest != null)
                {
                    LabTestViewModel viewModel = new LabTestViewModel();
                    viewModel.UpdateGridVisibility = true;
                    viewModel.LabTestListVisibility = false;
                    viewModel.CreateGridVisibility = false;
                    viewModel.ResetUpdateButtonsVisibility = true;
                    DataContext = viewModel;
                    ((LabTestViewModel)DataContext).SelectedId = selectedItemId;

                    HospitalName.IsEnabled = true;
                    TestName.IsEnabled = true;
                    Cost.IsEnabled = true;
                    Discount.IsEnabled = true;
                    LabPaid.IsEnabled = false;
                    Status.IsEnabled = true;
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
                LabTest labTest = labTestRepository.GetByID(selectedItemId);
                if (labTest != null)
                {
                    deleteConfirmationPopup.IsOpen = true;
                    DeletePopupText.Text = "Do you want to delete " + labTest.TestName + "?";
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
            LabTestViewModel viewModel = new LabTestViewModel();
            viewModel.UpdateGridVisibility = false;
            viewModel.LabTestListVisibility = false;
            viewModel.CreateGridVisibility = true;
            DataContext = viewModel;
            LabPaidCreate.IsEnabled = false;
        }

        private void ViewDetails(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            var selectedRowData = (QWellApp.Models.LabTestView)dataGrid.SelectedItem;

            if (selectedRowData != null)
            {
                LabTestViewModel viewModel = new LabTestViewModel();
                viewModel.UpdateGridVisibility = true;
                viewModel.LabTestListVisibility = false;
                viewModel.CreateGridVisibility = false;
                viewModel.ResetUpdateButtonsVisibility = false;
                DataContext = viewModel;

                ((LabTestViewModel)DataContext).SelectedId = selectedRowData.Id;
                ((LabTestViewModel)DataContext).GetLabTestDetails.Execute(null);

                HospitalName.IsEnabled = false;
                TestName.IsEnabled = false;
                Cost.IsEnabled = false;
                Discount.IsEnabled = false;
                LabPaid.IsEnabled = false;
                Status.IsEnabled = false;
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9\.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
