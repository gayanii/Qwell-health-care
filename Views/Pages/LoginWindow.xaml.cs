using QWell;
using QWellApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QWellApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private MainViewModel mainViewModel;

        public LoginWindow()
        {
            InitializeComponent();
            //mainViewModel = new MainViewModel();
        }

        private void LoginBtnClicked (object sender, RoutedEventArgs e)
        {
            LoginViewModel viewModel = new LoginViewModel();
            ((LoginViewModel)DataContext).LoadingGridVisibility = true;
            loadingGrid.Visibility = Visibility.Visible;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(passwordBox.Password) && passwordBox.Password.Length > 0)
            //textPassword.Visibility = Visibility.Collapsed;
            //else
            txtPassword.Visibility = Visibility.Visible;
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void enterClicked(object sender, RoutedEventArgs e) { }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(passwordBox.Password))
            //{
            //MessageBox.Show("Successfully Signed In");
            //}
        }

        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            //textEmail.Visibility = Visibility.Collapsed;
            //else
            //textEmail.Visibility = Visibility.Visible;
        }

        private void textUsername_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Focus();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //mainViewModel = new MainViewModel();
            //((MainViewModel)DataContext).IsMainViewVisible = false;
            var mainWindow = new MainWindow();
            mainWindow.Close();
            var loginWindow = new LoginWindow();
            loginWindow.Close();
            if (mainWindow.IsVisible == false)
            {
                loginWindow = new LoginWindow();
                loginWindow.Close();
            }
            Application.Current.Shutdown();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }
    }
}
