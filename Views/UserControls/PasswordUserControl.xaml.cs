using QWellApp.ViewModels;
using QWellApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

namespace QWellApp.UserControls
{
    /// <summary>
    /// Interaction logic for PasswordUserControl.xaml
    /// </summary>
    public partial class PasswordUserControl : UserControl
    {
        public static readonly DependencyProperty PasswordProperty =
    DependencyProperty.Register(
        "Password",
        typeof(SecureString),
        typeof(PasswordUserControl),
        new PropertyMetadata(null, OnPasswordChangedFromVM));

        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public PasswordUserControl()
        {
            InitializeComponent();
            txtPassword.PasswordChanged += OnPasswordChanged;
        }
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = txtPassword.SecurePassword;
        }

        private static void OnPasswordChangedFromVM(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PasswordUserControl;
            var newValue = e.NewValue as SecureString;

            // If VM sets an *empty* SecureString → clear UI
            if (newValue != null && newValue.Length == 0)
            {
                control.txtPassword.Clear();
            }

            // Also handle null case if needed
            if (newValue == null)
            {
                control.txtPassword.Clear();
            }
        }

        public void aa()
        {
            EmployeeViewModel dd = new EmployeeViewModel();
            DataContext = dd;
            txtPassword.Clear();
        }

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(PasswordUserControl));

    }
}
