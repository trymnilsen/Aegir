using Aegir.ViewModel.NodeProxy;
using Aegir.ViewModel.Timeline;
using GalaSoft.MvvmLight.Ioc;

namespace Aegir.ViewModel
{
    public class ViewModelLocator
    {
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
        /// Helper method for getting the objecttreeviewmodel
        /// </summary>
        public ScenegraphViewModel ScenegraphVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ScenegraphViewModel>();
            }
        }

        public MapViewModel MapVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MapViewModel>();
            }
        }

        public SelectionViewModel SelectionVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SelectionViewModel>();
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