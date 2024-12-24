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
using QWellApp.Repositories;
using QWellApp.Models;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using Microsoft.Win32;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SupplierView.xaml
    /// </summary>
    public partial class SupplierView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        private ISupplierRepository supplierRepository;
        private ViewModelBase CurrentChildView;
        private SupplierViewModel supplierViewModel;
        public SupplierView()
        {
            InitializeComponent();
            supplierViewModel = new SupplierViewModel();
            DataContext = supplierViewModel;
            supplierRepository = new SupplierRepository();
            // Attach the TextChanged event handler for search
            SearchBox.TextChanged += Search_TextChanged;
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

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((SupplierViewModel)DataContext).LoadSearchResults.Execute(null);
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (QWellApp.Models.SupplierView)dataGrid.SelectedItem;
                ((SupplierViewModel)DataContext).SelectedId = selectedRowData.Id;
                selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void UpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                Supplier supplier = supplierRepository.GetByID(selectedItemId);
                if (supplier != null)
                {
                    SupplierViewModel viewModel = new SupplierViewModel();
                    viewModel.UpdateGridVisibility = true;
                    viewModel.SupplierListVisibility = false;
                    viewModel.CreateGridVisibility = false;
                    viewModel.ResetUpdateButtonsVisibility = true;
                    DataContext = viewModel;
                    ((SupplierViewModel)DataContext).SelectedId = selectedItemId;
                    
                    Company.IsEnabled = true;
                    Email.IsEnabled = true;
                    Address.IsEnabled = true;
                    TelephoneNum.IsEnabled = true;
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
                Supplier supplier = supplierRepository.GetByID(selectedItemId);
                if (supplier != null)
                {
                    deleteConfirmationPopup.IsOpen = true;
                    DeletePopupText.Text = "Do you want to delete " + supplier.CompanyName + "?";
                } else
                {
                    MessageBox.Show("Select an item first!");
                }
            }
            else
            {
                MessageBox.Show("Select an item first!");
            }
        }

        private void CreateButtonClicked(object sender, RoutedEventArgs e)
        {
            SupplierViewModel viewModel = new SupplierViewModel();
            viewModel.UpdateGridVisibility = false;
            viewModel.SupplierListVisibility = false;
            viewModel.CreateGridVisibility = true;
            DataContext = viewModel;
        }

        private void ViewDetails(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            var selectedRowData = (QWellApp.Models.SupplierView)dataGrid.SelectedItem;

            if (selectedRowData != null)
            {
                SupplierViewModel viewModel = new SupplierViewModel();
                viewModel.UpdateGridVisibility = true;
                viewModel.SupplierListVisibility = false;
                viewModel.CreateGridVisibility = false;
                viewModel.ResetUpdateButtonsVisibility = false;
                DataContext = viewModel;

                ((SupplierViewModel)DataContext).SelectedId = selectedRowData.Id;
                ((SupplierViewModel)DataContext).GetSupplierDetails.Execute(null);

                Company.IsEnabled = false;
                Email.IsEnabled = false;
                Address.IsEnabled = false;
                TelephoneNum.IsEnabled = false;
                Status.IsEnabled = false;
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
