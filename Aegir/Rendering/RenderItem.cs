using AegirCore.Behaviour.World;
using AegirCore.Mesh;
using System;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class RenderItem : IDisposable
    {
        private bool IsGeometryDirty;
        private IGeometryFactory geometryFactory;
        private MeshData meshData;

        private MeshGeometry3D geometry;

        public MeshGeometry3D Geometry
        {
            get
            {
                if (IsGeometryDirty || geometry == null)
                {
                    geometry = geometryFactory.GetGeometry(meshData);
                    IsGeometryDirty = false;
                }
                return geometry;
            }
        }

        public Transform Transform { get; private set; }

        public RenderItem(IGeometryFactory geometryFactory, MeshData meshData, AegirCore.Behaviour.World.Transform transform)
        {
            this.geometryFactory = geometryFactory;
            this.IsGeometryDirty = true;
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
            IsGeometryDirty = true;
            TriggerGeometryChanged();
        }

        public void Dispose()
        {
            this.meshData.VerticePositionsChanged -= MeshData_VerticePositionsChanged;
        }

        private void TriggerTransformChanged()
        {
            TransformationChangedHandler transformHandler = TransformChanged;
            if (transformHandler != null)
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