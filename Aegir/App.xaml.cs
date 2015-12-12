using System.Windows;

namespace Aegir
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Shell appShell;

        public App()
        {
            var foo = new ViewModel.ViewModelLocator();
            appShell = new Shell();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            appShell.ShellLoaded();
        }
    }
}