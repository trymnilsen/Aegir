using Aegir.ViewModel.Timeline;
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
            TimelineViewModel timeViewModel = DataContext as TimelineViewModel;

            TimelineConfig configuration = new TimelineConfig(timeViewModel.TimelineStart,
                                                              timeViewModel.TimelineEnd,
                                                              timeViewModel.PlaybackStart,
                                                              timeViewModel.PlaybackEnd,
                                                              TimelineTickDisplayMode.Ticks,
                                                              timeViewModel.LoopPlayback,
                                                              timeViewModel.Reverse);

            TimelineConfigWindow timeConfigWindow = new TimelineConfigWindow(configuration);
            timeConfigWindow.ShowDialog();

            timeViewModel.TimelineStart = configuration.TimelineViewStart;
            timeViewModel.TimelineEnd = configuration.TimelineViewEnd;
            timeViewModel.PlaybackStart = configuration.PlaybackStart;
            timeViewModel.PlaybackEnd = configuration.PlaybackEnd;
            timeViewModel.LoopPlayback = configuration.Loop;
            timeViewModel.Reverse = configuration.Reverse;
        }
    }
}