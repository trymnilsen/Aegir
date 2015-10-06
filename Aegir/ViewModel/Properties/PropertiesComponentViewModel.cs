using AegirCore.Component;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Properties
{
    public class PropertiesComponentViewModel
    {
        /// <summary>
        /// Component attached to actor
        /// </summary>
        public Component Component { get; set; }
        public string ComponentName
        {
            get { return Component.Name;  }
        }
        /// <summary>
        /// Command to call when a component has been requested removed
        /// </summary>
        public RelayCommand RemoveComponentCommand { get; private set; }

        /// <summary>
        /// Create a new viewmodel for a component
        /// </summary>
        /// <param name="component"></param>
        public PropertiesComponentViewModel(Component component)
        {
            this.Component = component;
            this.RemoveComponentCommand = new RelayCommand(this.RemoveComponent);
        }
        private void RemoveComponent()
        {
            ComponentRemovedEventHandler Removed = ComponentRemoved;
            if(Removed!=null)
            {
                Removed(this, new ComponentRemovedEventArgs(this.Component));
            }
        }
        public delegate void ComponentRemovedEventHandler(object sender, ComponentRemovedEventArgs e);
        public event ComponentRemovedEventHandler ComponentRemoved;
    }
}
