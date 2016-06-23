using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Aegir.View.Timeline
{
    /// <summary>
    /// Interaction logic for TimelineConfig.xaml
    /// </summary>
    public partial class TimelineConfigWindow : Window
    {
        public TimelineConfigWindow(TimelineConfig config)
        { 
            InitializeComponent();
            DataContext = config;
        }


        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            TimelineConfig config = DataContext as TimelineConfig;
            if(config!=null)
            {
                string errorMessage = "";
                if(!config.Validate(out errorMessage))
                {
                    MessageBox.Show(errorMessage);
                    e.Cancel = true;
                }
            }
        }
    }
}
