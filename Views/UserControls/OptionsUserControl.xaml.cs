using FontAwesome.WPF;
using Microsoft.VisualBasic.FileIO;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QWellApp.UserControls
{
    /// <summary>
    /// Interaction logic for OptionsUserControl.xaml
    /// </summary>
    public partial class OptionsUserControl : UserControl
    {
        public OptionsUserControl()
        {
            InitializeComponent();
            SetFocusByTag("Male");
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(OptionsUserControl));


        public FontAwesome.WPF.FontAwesomeIcon Icon
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(OptionsUserControl));

        private void selectedGender(object sender, MouseButtonEventArgs e)
        {
            var aa = sender;
            ImageAwesome ss = sender as ImageAwesome;
            var dd = ss.Icon;
        }

        private void SetFocusByTag(string buttonName)
        {
            foreach (var child in LogicalTreeHelper.GetChildren(stackPanel))
            {
                if (child is FrameworkElement frameworkElement && frameworkElement.Tag != null && frameworkElement.Tag.ToString() == buttonName)
                {
                    frameworkElement.Focus();
                    break;
                }
            }
        }
    }
}
