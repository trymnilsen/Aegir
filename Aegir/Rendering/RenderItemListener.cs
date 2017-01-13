using AegirCore.Behaviour.World;
using System;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class RenderItemListener : IDisposable
    {
        public Visual3D Visual { get; private set; }

        public Transform Item { get; private set; }

        public RenderItemListener(Visual3D visual, AegirCore.Behaviour.World.Transform item)
        {
            this.Visual = visual;
            this.Item = item;
        }

        public void Invalidate()
        {
            Transform3D transformation = GetVisualTransformation(Item);
            Visual.Dispatcher.Invoke(() =>
            {
                if (Visual.Transform != transformation)
                {
                    Visual.Transform = transformation;
                }
            });
        }

        private void MeshData_VerticePositionsChanged()
        {
            throw new NotImplementedException();
        }

        private Transform3D GetVisualTransformation(AegirCore.Behaviour.World.Transform transform)
        {
           
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}