using Aegir.ViewModel.Properties;
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
            //#if DEBUG
            //    if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            //#endif

            SimpleIoc.Default.Register<StatusBarViewModel>(true);
            SimpleIoc.Default.Register<PropertiesViewModel>(true);
            SimpleIoc.Default.Register<ObjectTreeViewModel>(true);
            SimpleIoc.Default.Register<MenuStripViewModel>(true);
            SimpleIoc.Default.Register<PlaybackViewModel>(true);
            SimpleIoc.Default.Register<RenderingViewModel>(true);
            SimpleIoc.Default.Register<MapViewModel>(true);

        }

        /// <summary>
        /// Gets the Statusbar view's viewmodel
        /// </summary>

        public StatusBarViewModel StatusBarVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<StatusBarViewModel>();
            }
        }
        /// <summary>
        /// Gets the Menubar view's viewmodel
        /// </summary>

        public MenuStripViewModel MenuStripVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MenuStripViewModel>();
            }
        }
        /// <summary>
        /// Gets the Menubar view's viewmodel
        /// </summary>

        public PlaybackViewModel PlaybackVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PlaybackViewModel>();
            }
        }
        /// <summary>
        /// Gets the properties viewmodel
        /// </summary>
        public PropertiesViewModel PropertiesVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PropertiesViewModel>();
            }
        }
        /// <summary>
        /// Helper method for getting the objecttreeviewmodel 
        /// </summary>
        public ObjectTreeViewModel ObjectTreeVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ObjectTreeViewModel>();
            }
        }
        public MapViewModel MapVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MapViewModel>();
            }
        }
        public RenderingViewModel RenderingVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<RenderingViewModel>();
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