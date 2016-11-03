using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ViewPropertyGrid.PropertyGrid
{
    public class ValueControl
    {
        public FrameworkElement Control { get; private set; }
        public EditingBehaviour EditBehaviour { get; private set; }
        public Action onEditStart;
        public Action onEditEnd;

        public ValueControl(FrameworkElement control, 
                            EditingBehaviour behaviour = EditingBehaviour.AlwaysVisible,
                            Action onEditStart = null,
                            Action onEditEnd = null)
        {
            Control = control;
            EditBehaviour = behaviour;
            this.onEditStart = onEditStart;
            this.onEditEnd = onEditEnd;
        }


        /// <summary>
        /// Turns on incomming changes for bindings
        /// </summary>
        internal void EditEnd()
        {
            onEditEnd?.Invoke();
        }
        /// <summary>
        /// Suspends incomming changes for the binding
        /// </summary>
        internal void EditStart()
        {
            onEditStart?.Invoke();
        }

    }
}