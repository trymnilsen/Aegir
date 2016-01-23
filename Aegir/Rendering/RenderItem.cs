using AegirCore.Behaviour.World;
using AegirCore.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class RenderItem : IDisposable
    {
        private MeshData meshData;

        public MeshGeometry3D Geometry { get; private set; }
        public TransformBehaviour Transform { get; private set; }

        public RenderItem(MeshData meshData, TransformBehaviour transform)
        {
            this.meshData = meshData;
            Transform = transform;
            this.meshData.VerticePositionsChanged += MeshData_VerticePositionsChanged;
        }

        public void Invalidate()
        {
            TriggerTransformChanged();
        }

        private void MeshData_VerticePositionsChanged()
        {
            TriggerGeometryChanged();
        }

        public void Dispose()
        {
            this.meshData.VerticePositionsChanged -= MeshData_VerticePositionsChanged;
        }

        private void TriggerTransformChanged()
        {
            TransformationChangedHandler transformHandler = TransformChanged;
            if(transformHandler != null)
            {
                transformHandler();
            }
        }
        private void TriggerGeometryChanged()
        {
            GeometryChangedHandler geometryHandler = MeshGeometryChanged;
            if (geometryHandler != null)
            {
                geometryHandler();
            }
        }
        public delegate void TransformationChangedHandler();
        public event TransformationChangedHandler TransformChanged;

        public delegate void GeometryChangedHandler();
        public event GeometryChangedHandler MeshGeometryChanged;
    }
}
