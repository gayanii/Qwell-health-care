using iText.Kernel.Pdf;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using QWellApp.ViewModels;
using System;
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
using iText.Layout.Element;
using Microsoft.Win32;
using System.Globalization;

namespace QWellApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CommissionView.xaml
    /// </summary>
    public partial class ChangePasswordView : UserControl
    {
        public int selectedItemId;
        private bool isItemSelected = false;
        private ViewModelBase CurrentChildView;
        private ChangePasswordViewModel stockViewModel;

        public ChangePasswordView()
        {
            InitializeComponent();
            stockViewModel = new ChangePasswordViewModel();
            DataContext = stockViewModel;
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
