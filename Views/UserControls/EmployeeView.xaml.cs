using FontAwesome.Sharp;
using FontAwesome.WPF;
using QWell;
using QWellApp.DBConnection;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.UserControls;
using QWellApp.ViewModels;
using QWellApp.ViewModels.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        public bool refresh;
        private IUserRepository userRepository;
        private ViewModelBase CurrentChildView;
        private EmployeeViewModel employeeViewModel;

        public EmployeeView()
        {
            InitializeComponent();
            employeeViewModel = new EmployeeViewModel();
            DataContext = employeeViewModel;
            userRepository = new UserRepository();
            ((EmployeeViewModel)DataContext).LoadSearchResults.Execute(null);
            // Attach the TextChanged event handler for search
            SearchBox.TextChanged += Search_TextChanged;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((EmployeeViewModel)DataContext).LoadSearchResults.Execute(null);
        }

        private void rowChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
            {
                var selectedRowData = (UserView)dataGrid.SelectedItem;
                ((EmployeeViewModel)DataContext).SelectedId = selectedRowData.Id;
                selectedItemId = selectedRowData.Id;
                isItemSelected = true;
            }
        }

        private void UpdateButtonClicked (object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                UserDetails user = userRepository.GetByID(selectedItemId);
                if (user != null)
                {
                    EmployeeViewModel viewModel = new EmployeeViewModel();
                    viewModel.UpdateGridVisibility = true;
                    viewModel.UserListVisibility = false;
                    viewModel.CreateGridVisibility = false;
                    viewModel.ResetUpdateButtonsVisibility = true;
                    DataContext = viewModel;
                    ((EmployeeViewModel)DataContext).SelectedId = selectedItemId;

                    FirstName.IsEnabled = true;
                    LastName.IsEnabled = true;
                    Username.IsEnabled = true;
                    Email.IsEnabled = true;
                    MobileNum.IsEnabled = true;
                    Telephone.IsEnabled = true;
                    NIC.IsEnabled = true;
                    Type.IsEnabled = true;
                    Role.IsEnabled = true;
                    Status.IsEnabled = true;
                    Male.IsEnabled = true;
                    Female.IsEnabled = true;
                }
            } else
            {
                MessageBox.Show("Select an item first!");
            }
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isItemSelected)
            {
                UserDetails user = userRepository.GetByID(selectedItemId);
                if (user != null)
                {
                    deleteConfirmationPopup.IsOpen = true;
                    DeletePopupText.Text = "Do you want to delete " + user.FirstName + "?";
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
            EmployeeViewModel viewModel = new EmployeeViewModel();
            viewModel.UpdateGridVisibility = false;
            viewModel.UserListVisibility = false;
            viewModel.CreateGridVisibility = true;
            DataContext = viewModel;
        }

        //Update functions
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void selectedGender(object sender, MouseButtonEventArgs e)
        {
            var aa = sender;
            ImageAwesome ss = sender as ImageAwesome;
            var dd = ss.Icon;
        }

        private void genderGotFocus(object sender, RoutedEventArgs e)
        {
            UserDetails user = userRepository.GetByID(selectedItemId);
            if (user != null)
            {
                if (user.Gender == "M")
                {
                    Male.Focus();
                }
                else
                {
                    Female.Focus();
                }
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //EmployeeView nn = new EmployeeView();
            //updateView.Visibility = Visibility.Collapsed;
            //listView.Visibility = Visibility.Visible;
            //InitializeComponent();

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //PasswordCreate.Clear();
            //PasswordUserControl xx = new PasswordUserControl();
            //xx.aa();
            //DataContext = xx;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Assuming your DataContext is set to an instance of MyViewModel
            var viewModel = DataContext as EmployeeViewModel;

            if (viewModel != null)
            {
                viewModel.Password = ((PasswordBox)sender).SecurePassword;
            }
        }

        private void ViewDetails(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            var selectedRowData = (UserView)dataGrid.SelectedItem;

            if (selectedRowData != null)
            {
                EmployeeViewModel viewModel = new EmployeeViewModel();
                viewModel.UpdateGridVisibility = true;
                viewModel.UserListVisibility = false;
                viewModel.CreateGridVisibility = false;
                viewModel.ResetUpdateButtonsVisibility = false;
                DataContext = viewModel;

                ((EmployeeViewModel)DataContext).SelectedId = selectedRowData.Id;
                ((EmployeeViewModel)DataContext).GetUserDetails.Execute(null);

                FirstName.IsEnabled = false;
                LastName.IsEnabled = false;
                Username.IsEnabled = false;
                Email.IsEnabled = false;
                MobileNum.IsEnabled = false;
                Telephone.IsEnabled = false;
                NIC.IsEnabled = false;
                Type.IsEnabled = false;
                Role.IsEnabled = false;
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
