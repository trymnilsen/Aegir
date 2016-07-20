using Aegir.ViewModel;
using Aegir.ViewModel.NodeProxy;
using Aegir.ViewModel.Properties;
using Aegir.ViewModel.Timeline;
using Aegir.Windows;
using AegirCore;
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
        private ApplicationContext application;

        public App()
        {
            log.Debug("Starting Application");
            application = new ApplicationContext();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            application.Init();

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            log.FatalFormat("Fatal Error Occured in Application: \n{0}", e.Exception);
        }

        private void SetupViewModels()
        {

            SimpleIoc.Default.Register<StatusBarViewModel>(
                () => {
                    return new StatusBarViewModel() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<PropertiesViewModel>(
                () => {
                    return new PropertiesViewModel() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<MenuStripViewModel>(
                () => {
                    return new MenuStripViewModel() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<PlaybackViewModel>(
                () => {
                    return new PlaybackViewModel() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<MapViewModel>(
                () => {
                    return new MapViewModel() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<ScenegraphViewModelProxy>(
                () => {
                    return new ScenegraphViewModelProxy() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<TimelineViewModel>(
                () => {
                    return new TimelineViewModel() { Messenger = application.MessageHub };
                }
            , true);
        }
    }
}