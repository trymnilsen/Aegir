using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.PropertyGrid
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomEditorAttribute : Attribute
    {
    }
}
