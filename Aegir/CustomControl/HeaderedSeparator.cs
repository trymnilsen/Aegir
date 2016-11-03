using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Aegir.CustomControl
{
    public class HeaderedSeparator : Control
    {
        public static DependencyProperty HeaderProperty =
            DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(HeaderedSeparator), new PropertyMetadata("N/A", (a,b)=> { Debug.WriteLine("Fooooobar" + b.NewValue); }));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
    }
}