using AegirLib;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            AegirIOC.Register(new StatusBarViewModel());
            AegirIOC.Register(new PropertiesViewModel());
            AegirIOC.Register(new ObjectTreeViewModel());
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

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {

        }
    }
}