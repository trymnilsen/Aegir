using GMap.NET;
using GMap.NET.MapProviders;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aegir.View
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();
        }

        private void mapControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                mapControl.MapProvider = GoogleMapProvider.Instance;
                mapControl.Manager.Mode = GMap.NET.AccessMode.ServerAndCache;

                GMapProvider.WebProxy = null;
                mapControl.Position = new PointLatLng(59.755786121111, 11);

                mapControl.MinZoom = 1;
                mapControl.MaxZoom = 20;
                mapControl.Zoom = 8;

                mapControl.DragButton = MouseButton.Left;
            }
        }
    }
}