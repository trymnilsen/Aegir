using System.Windows;
using System.Windows.Controls;

namespace ViewPropertyGrid.PropertyGrid
{
    public interface IControlProvider
    {
        FrameworkElement GetControl(InspectableProperty property);
    }
}