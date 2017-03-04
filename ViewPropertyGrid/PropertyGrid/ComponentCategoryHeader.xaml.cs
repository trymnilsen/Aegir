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

namespace ViewPropertyGrid.PropertyGrid
{
    /// <summary>
    /// Interaction logic for ComponentCategoryHeader.xaml
    /// </summary>
    public partial class ComponentCategoryHeader : UserControl
    {
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(ComponentCategoryHeader), new PropertyMetadata("No Header"));


        public ComponentCategoryHeader()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RemoveClicked?.Invoke();
        }

        public delegate void RemoveClickedHandler();
        public event RemoveClickedHandler RemoveClicked;
    }
}
