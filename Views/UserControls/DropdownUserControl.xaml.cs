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

namespace QWellApp.UserControls
{
    /// <summary>
    /// Interaction logic for DropdownUserControl.xaml
    /// </summary>
    public partial class DropdownUserControl : UserControl
    {
        public DropdownUserControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ArrayItemsSource", typeof(List<string>), typeof(DropdownUserControl));

        public List<string> ArrayItemsSource
        {
            get { return (List<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(DropdownUserControl));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
    }
}
