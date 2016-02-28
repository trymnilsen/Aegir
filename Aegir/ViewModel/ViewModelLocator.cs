using Aegir.ViewModel.NodeProxy;
using Aegir.ViewModel.Properties;
using Aegir.ViewModel.Timeline;
using GalaSoft.MvvmLight.Ioc;

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
            SimpleIoc.Default.Register<MenuStripViewModel>(true);
            SimpleIoc.Default.Register<PlaybackViewModel>(true);
            SimpleIoc.Default.Register<MapViewModel>(true);
            SimpleIoc.Default.Register<ScenegraphViewModelProxy>(true); 
            SimpleIoc.Default.Register<TimelineViewModel>(true);
        }
        public TimelineViewModel TimelineVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<TimelineViewModel>();
            }
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
        public ScenegraphViewModelProxy ScenegraphVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ScenegraphViewModelProxy>();
            }
        }

        public MapViewModel MapVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MapViewModel>();
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