﻿using Aegir.Util;
using Aegir.ViewModel;
using Aegir.ViewModel.EntityProxy;
using Aegir.ViewModel.Statusbar;
using Aegir.ViewModel.Timeline;
using Aegir.Windows;
using AegirLib;
using GalaSoft.MvvmLight.Ioc;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using TinyMessenger;

namespace Aegir
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ApplicationContext application;
        public static Stopwatch stopwatch;

        public App()
        {
            stopwatch = Stopwatch.StartNew();
            Aegir.Util.DebugUtil.LogWithLocation("Starting Application");
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
            Aegir.Util.DebugUtil.LogWithLocation($"Expection: Sender {sender?.ToString()} ex: {e.ToString()}");
        }

        private void SetupViewModels()
        {
            SimpleIoc.Default.Register<StatusBarViewModel>(
                () =>
                {
                    return new StatusBarViewModel() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<MenuStripViewModel>(
                () =>
                {
                    return new MenuStripViewModel() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<PlaybackViewModel>(
                () =>
                {
                    return new PlaybackViewModel() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<MapViewModel>(
                () =>
                {
                    return new MapViewModel() { Messenger = application.MessageHub };
                }
            , true);
            SimpleIoc.Default.Register<ScenegraphViewModel>(
                () =>
                {
                    return new ScenegraphViewModel(application.MessageHub as TinyMessengerHub);
                }
            , true);
            SimpleIoc.Default.Register<TimelineViewModel>(
                () =>
                {
                    return new TimelineViewModel(application.MessageHub, application.Engine.KeyframeEngine);
                }
            , true);
            SimpleIoc.Default.Register<SelectionViewModel>(
                () =>
                {
                    return new SelectionViewModel(application.MessageHub);
                }, true);
        }
    }
}