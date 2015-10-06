using GMap.NET;
using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                mapControl.MapProvider = OpenSeaMapHybridProvider.Instance;
                mapControl.Manager.Mode = GMap.NET.AccessMode.ServerAndCache;

                GMapProvider.WebProxy = null;
                mapControl.Position = new PointLatLng(55.755786121111, 37.617633343333);

                mapControl.MinZoom = 1;
                mapControl.MaxZoom = 20;
                mapControl.Zoom = 10;

                mapControl.DragButton = MouseButton.Left;
            }
        }
    }
}
