using Aegir.Messages.Project;
using Aegir.Util;
using Microsoft.Win32;
using System.Windows;

namespace Aegir
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.stopwatch.Stop();
            DebugUtil.LogWithLocation($"Startup time {App.stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        private void FileOpenClick(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MessageBoxResult closeConfirmDialog = MessageBox.Show("Are you sure you want to quit?", "Confirm Close", MessageBoxButton.YesNo);
            //if (closeConfirmDialog == MessageBoxResult.No)
            //{
            //    e.Cancel = true;
            //}
        }
    }
}