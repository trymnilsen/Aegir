using AegirLib.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Properties
{
    /// <summary>
    /// Arguments for a "Remove Component of currently active actor" event
    /// </summary>
    public class ComponentRemovedEventArgs
    {
        /// <summary>
        /// The Component that is to be removed
        /// </summary>
        public Component Removed { get; private set; }
        /// <summary>
        /// Create new event args object
        /// </summary>
        /// <param name="componentToRemove">The component to remove from the current actor</param>
        public ComponentRemovedEventArgs(Component componentToRemove)
        {

        }
    }
}
