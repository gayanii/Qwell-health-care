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
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        public bool refresh;
        private IUserRepository userRepository;
        private IProductRepository productRepository;
        private ViewModelBase CurrentChildView;
        private ProductViewModel productViewModel;

        public ProductView()
        {
            InitializeComponent();
            productViewModel = new ProductViewModel();
            DataContext = productViewModel;
            userRepository = new UserRepository();
            productRepository = new ProductRepository();
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
            ((ProductViewModel)DataContext).LoadSearchResults.Execute(null);
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (QWellApp.Models.ProductView)dataGrid.SelectedItem;
                ((ProductViewModel)DataContext).SelectedId = selectedRowData.Id;
                selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void UpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                Product product = productRepository.GetByID(selectedItemId);
                if (product != null)
                {
                    ProductViewModel viewModel = new ProductViewModel();
                    viewModel.UpdateGridVisibility = true;
                    viewModel.ProductListVisibility = false;
                    viewModel.CreateGridVisibility = false;
                    viewModel.ResetUpdateButtonsVisibility = true;
                    DataContext = viewModel;
                    ((ProductViewModel)DataContext).SelectedId = selectedItemId;

                    BrandName.IsEnabled = true;
                    Generic.IsEnabled = true;
                    CurrentQuantity.IsEnabled = true;
                    SelllingPrice.IsEnabled = true;
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
                Product product = productRepository.GetByID(selectedItemId);
                if (product != null)
                {
                    deleteConfirmationPopup.IsOpen = true;
                    DeletePopupText.Text = "Do you want to delete " + product.BrandName + "?";
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
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.UpdateGridVisibility = false;
            viewModel.ProductListVisibility = false;
            viewModel.CreateGridVisibility = true;
            DataContext = viewModel;
        }

        private void ViewDetails(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            var selectedRowData = (QWellApp.Models.ProductView)dataGrid.SelectedItem;

            if (selectedRowData != null)
            {
                ProductViewModel viewModel = new ProductViewModel();
                viewModel.UpdateGridVisibility = true;
                viewModel.ProductListVisibility = false;
                viewModel.CreateGridVisibility = false;
                viewModel.ResetUpdateButtonsVisibility = false;
                DataContext = viewModel;

                ((ProductViewModel)DataContext).SelectedId = selectedRowData.Id;
                ((ProductViewModel)DataContext).GetProductDetails.Execute(null);

                BrandName.IsEnabled = false;
                Generic.IsEnabled = false;
                CurrentQuantity.IsEnabled = false;
                SelllingPrice.IsEnabled = false;
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
