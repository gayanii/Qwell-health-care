using FontAwesome.WPF;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels;
using QWellApp.ViewModels.Common;
using System;
using System.Collections;
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

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ProductRecordView.xaml
    /// </summary>
    public partial class ProductRecordView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        public bool refresh;
        private IUserRepository userRepository;
        private IProductRecordRepository productRecordRepository;
        private ViewModelBase CurrentChildView;
        private ProductRecordViewModel productRecordViewModel;

        public ProductRecordView()
        {
            InitializeComponent();
            productRecordViewModel = new ProductRecordViewModel();
            DataContext = productRecordViewModel;
            userRepository = new UserRepository();
            productRecordRepository = new ProductRecordRepository();
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
            ((ProductRecordViewModel)DataContext).LoadSearchResults.Execute(null);
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (QWellApp.Models.ProductRecordView)dataGrid.SelectedItem;
                ((ProductRecordViewModel)DataContext).SelectedId = selectedRowData.Id;
                selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void UpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                ProductRecord productRecord = productRecordRepository.GetByID(selectedItemId);
                if (productRecord != null)
                {
                    ProductRecordViewModel viewModel = new ProductRecordViewModel();
                    viewModel.UpdateGridVisibility = true;
                    viewModel.ProductListVisibility = false;
                    viewModel.CreateGridVisibility = false;
                    viewModel.ResetUpdateButtonsVisibility = true;
                    DataContext = viewModel;
                    ((ProductRecordViewModel)DataContext).SelectedId = selectedItemId;

                    Barcode.IsEnabled = true;
                    ProductName.IsEnabled = true;
                    SupplierPrice.IsEnabled = true;
                    SellingPrice.IsEnabled = true;
                    OrderedQuantity.IsEnabled = true;
                    ExpDate.IsEnabled = true;
                    ReceivedDate.IsEnabled = true;
                    Supplier.IsEnabled = true;
                    AddedBy.IsEnabled = false;
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
                ProductRecord productRecord = productRecordRepository.GetByID(selectedItemId);
                if (productRecord != null)
                {
                    deleteConfirmationPopup.IsOpen = true;
                    DeletePopupText.Text = "Do you want to delete " + productRecord.Product.BrandName + " " + productRecord.Product.Generic + "?";
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
            ProductRecordViewModel viewModel = new ProductRecordViewModel();
            viewModel.UpdateGridVisibility = false;
            viewModel.ProductListVisibility = false;
            viewModel.CreateGridVisibility = true;
            DataContext = viewModel;
        }

        private void ViewDetails(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            var selectedRowData = (QWellApp.Models.ProductRecordView)dataGrid.SelectedItem;

            if (selectedRowData != null)
            {
                ProductRecordViewModel viewModel = new ProductRecordViewModel();
                viewModel.UpdateGridVisibility = true;
                viewModel.ProductListVisibility = false;
                viewModel.CreateGridVisibility = false;
                viewModel.ResetUpdateButtonsVisibility = false;
                DataContext = viewModel;

                ((ProductRecordViewModel)DataContext).SelectedId = selectedRowData.Id;
                ((ProductRecordViewModel)DataContext).GetProductRecordDetails.Execute(null);

                Barcode.IsEnabled = false;
                ProductName.IsEnabled = false;
                SupplierPrice.IsEnabled = false;
                SellingPrice.IsEnabled = false;
                OrderedQuantity.IsEnabled = false;
                ExpDate.IsEnabled = false;
                ReceivedDate.IsEnabled = false;
                Supplier.IsEnabled = false;
                AddedBy.IsEnabled = false;
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
