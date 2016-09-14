using System.Windows;
using System.Windows.Controls;

namespace ViewPropertyGrid.PropertyGrid
{
    public interface IControlProvider
    {
        ValueControl GetControl(InspectableProperty property);
    }
}