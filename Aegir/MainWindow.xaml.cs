using Aegir.Messages.Project;
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
        }

        private void FileOpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Project Files (*.proj)|*.proj|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            bool? foo = openFileDialog.ShowDialog();

            if (foo.HasValue && foo.Value)
            {
                LoadProjectFile.Send(openFileDialog.FileName);
            }
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