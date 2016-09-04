using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Aegir.PropertyGrid
{
    public class PropertyContainer : Grid
    {
        public PropertyContainer(string propertyName)
        {
            //Add three columns
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
        }
    }
}
