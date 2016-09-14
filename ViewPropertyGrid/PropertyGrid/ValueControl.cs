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
        public ValueControl(FrameworkElement control, EditingBehaviour behaviour = EditingBehaviour.AlwaysVisible)
        {
            Control = control;
            EditBehaviour = behaviour;
        }
    }
}
