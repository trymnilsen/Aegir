using Aegir.ViewModel.Properties;
using AegirLib;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Aegir.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            #if DEBUG
                if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            #endif

            AegirIOC.Register(new StatusBarViewModel());
            AegirIOC.Register(new PropertiesViewModel());
            AegirIOC.Register(new ObjectTreeViewModel());
            AegirIOC.Register(new MenuStripViewModel());
            AegirIOC.Register(new PlaybackViewModel());
            AegirIOC.Register(new RenderingViewModel());
            AegirIOC.Register(new MapViewModel());
        }

        /// <summary>
        /// Gets the Statusbar view's viewmodel
        /// </summary>

        public StatusBarViewModel StatusBarVM
        {
            get
            {
                return AegirIOC.Get<StatusBarViewModel>();
            }
        }
        /// <summary>
        /// Gets the Menubar view's viewmodel
        /// </summary>

        public MenuStripViewModel MenuStripVM
        {
            get
            {
                return AegirIOC.Get<MenuStripViewModel>();
            }
        }
        /// <summary>
        /// Gets the Menubar view's viewmodel
        /// </summary>

        public PlaybackViewModel PlaybackVM
        {
            get
            {
                return AegirIOC.Get<PlaybackViewModel>();
            }
        }
        /// <summary>
        /// Gets the properties viewmodel
        /// </summary>
        public PropertiesViewModel PropertiesVM
        {
            get
            {
                return AegirIOC.Get<PropertiesViewModel>();
            }
        }
        /// <summary>
        /// Helper method for getting the objecttreeviewmodel 
        /// </summary>
        public ObjectTreeViewModel ObjectTreeVM
        {
            get
            {
                return AegirIOC.Get<ObjectTreeViewModel>();
            }
        }
        public MapViewModel MapVM
        {
            get
            {
                return AegirIOC.Get<MapViewModel>();
            }
        }
        public RenderingViewModel RenderingVM
        {
            get
            {
                return AegirIOC.Get<RenderingViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {

        }
    }
}