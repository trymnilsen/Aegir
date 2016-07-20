using Aegir.ViewModel;
using Aegir.ViewModel.NodeProxy;
using Aegir.ViewModel.Properties;
using Aegir.ViewModel.Timeline;
using Aegir.Windows;
using GalaSoft.MvvmLight.Ioc;
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
            appShell = new Shell();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            appShell.ShellLoaded();

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            log.FatalFormat("Fatal Error Occured in Application: \n{0}", e.Exception);
        }

        private void SetupViewModels()
        {

            SimpleIoc.Default.Register<StatusBarViewModel>(
                () => {
                    return new StatusBarViewModel() { Messenger = appShell.Context.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<PropertiesViewModel>(
                () => {
                    return new PropertiesViewModel() { Messenger = appShell.Context.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<MenuStripViewModel>(
                () => {
                    return new MenuStripViewModel() { Messenger = appShell.Context.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<PlaybackViewModel>(
                () => {
                    return new PlaybackViewModel() { Messenger = appShell.Context.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<MapViewModel>(
                () => {
                    return new MapViewModel() { Messenger = appShell.Context.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<ScenegraphViewModelProxy>(
                () => {
                    return new ScenegraphViewModelProxy() { Messenger = appShell.Context.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<TimelineViewModel>(
                () => {
                    return new TimelineViewModel() { Messenger = appShell.Context.MessageHub };
                }
            , true);
        }
    }
}