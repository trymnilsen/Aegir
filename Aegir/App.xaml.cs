using log4net;
using System.Windows;

namespace Aegir
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        private Shell appShell;

        public App()
        {
            log.Debug("Starting Application");
            var foo = new ViewModel.ViewModelLocator();
            appShell = new Shell();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            appShell.ShellLoaded();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            log.FatalFormat("Fatal Error Occured in Application: \n{0}",e.Exception);
        }
    }
}