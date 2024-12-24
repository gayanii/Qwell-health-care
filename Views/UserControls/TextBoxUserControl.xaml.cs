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
    /// Interaction logic for TextBoxUserControl.xaml
    /// </summary>
    public partial class TextBoxUserControl : UserControl
    {
        public TextBoxUserControl()
        {
            InitializeComponent();
            textBox.TextChanged += OnTextChanged;  // Hook up the TextChanged event
            Loaded += OnLoaded; // Hook up the Loaded event
        }

        // Dependency property to enable or disable the behavior
        public bool EnableDefaultZero
        {
            get { return (bool)GetValue(EnableDefaultZeroProperty); }
            set { SetValue(EnableDefaultZeroProperty, value); }
        }

        public static readonly DependencyProperty EnableDefaultZeroProperty =
            DependencyProperty.Register("EnableDefaultZero", typeof(bool), typeof(TextBoxUserControl), new PropertyMetadata(false));


        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(TextBoxUserControl));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBoxUserControl));

        // Update the dependency property whenever the text changes
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = textBox.Text;
        }

        // Logic to handle default zero behavior
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (EnableDefaultZero)
            {
                textBox.TextChanged += OnTextChangedWithDefaultZero;
            }
        }

        private void OnTextChangedWithDefaultZero(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "0";
                textBox.CaretIndex = textBox.Text.Length; // Place caret at the end
            }
        }
    }
}
