using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aegir.Module.Visualization
{
    /// <summary>
    /// Interaction logic for Preview3d.xaml
    /// </summary>
    public partial class Preview3d : UserControl
    {
        public Preview3d()
        {
            InitializeComponent();
        }

        private void view1_Loaded(object sender, RoutedEventArgs e)
        {
            if(DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }
            ModelVisual3D device3D = new ModelVisual3D();
            device3D.Content = Display3d("Content/cruiseship.obj");
            // Add to view port
            
            view1.Children.Add(device3D);
        }
        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                //Adding a gesture here
                //viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);

                //Import 3D model file
                ModelImporter import = new ModelImporter();

                //Load the 3D model file
                device = import.Load(model);
            }
            catch (Exception e)
            {
                // Handle exception in case can not file 3D model
                MessageBox.Show(e.ToString());
            }
            return device;
        }
    }
}
