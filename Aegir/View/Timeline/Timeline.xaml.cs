using System.Windows;
using System.Windows.Controls;

namespace Aegir.View.Timeline
{
    /// <summary>
    /// Interaction logic for Timeline.xaml
    /// </summary>
    public partial class Timeline : UserControl
    {
        public Timeline()
        {
            InitializeComponent();
        }

        private void TimeConfig_Click(object sender, RoutedEventArgs e)
        {
            TimelineConfig timeConfigWindow = new TimelineConfig();
            timeConfigWindow.ShowDialog();
        }
    }
}