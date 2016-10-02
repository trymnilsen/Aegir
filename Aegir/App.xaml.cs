using Aegir.ViewModel;
using Aegir.ViewModel.NodeProxy;
using Aegir.ViewModel.Timeline;
using Aegir.Windows;
using AegirCore;
using GalaSoft.MvvmLight.Ioc;
using log4net;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using TinyMessenger;
using System.Windows.Navigation;
using Aegir.Util;

namespace Aegir
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        private ApplicationContext application;
        public static Stopwatch stopwatch;
        public App()
        {
            stopwatch = Stopwatch.StartNew();
            log.Debug("Starting Application");
            application = new ApplicationContext();
            SetupViewModels();
            //SimpleIoc.Default.Register<TinyMessengerHub>(() => { return application.MessageHub; });
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            application.Init();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
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
            SimpleIoc.Default.Register<ScenegraphViewModel>(
                () => {
                    return new ScenegraphViewModel(application.MessageHub as TinyMessengerHub);
                }
            , true);
            SimpleIoc.Default.Register<TimelineViewModel>(
                () => {
                    return new TimelineViewModel(application.MessageHub, application.Engine.KeyframeEngine);
                }
            , true);
        }
    }
}