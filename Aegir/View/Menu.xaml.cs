using Aegir.Messages.Project;
using Microsoft.Win32;
using PropertyTools.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace Aegir.View
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void MenuItem_Open_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Project Files (*.aprj)|*.aprj|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            bool? foo = openFileDialog.ShowDialog();

            if (foo.HasValue && foo.Value)
            {
                LoadProjectFile.Send(openFileDialog.FileName);
            }
        }

        private void MenuItem_Save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Project Files (*.aprj)|*.aprj|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            bool? foo = openFileDialog.ShowDialog();

            if (foo.HasValue && foo.Value)
            {
                SaveProjectFile.Send(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Will shutdown the current application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Quit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //http://stackoverflow.com/a/2820377/394381
            Application.Current.Shutdown();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutDialog(Application.Current.MainWindow);
            dlg.Title = "About Aegir";
            dlg.UpdateStatus = "";

            dlg.ShowDialog();
        }
    }
}