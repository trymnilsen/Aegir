using Aegir.ViewModel.NodeProxy;
using AegirCore.Behaviour.Rendering;
using AegirCore.Mesh.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class VisualFactory
    {
        private Dictionary<RenderingMode, VisualProvider> visualProviders;
        public VisualFactory(Dictionary<RenderingMode,VisualProvider> providers)
        {
            visualProviders = providers;
        }

        public Geometry3D GetVisual(RenderDeclaration renderData, RenderingMode renderMode)
        {
            //If we don't have provider, give the default dummy visual
            if(!visualProviders.ContainsKey(renderMode) || !(renderData.MeshData == null))
            {
                return GetDummyVisual();
            }

            return visualProviders[renderMode].GetVisual(renderData.MeshData);

        }
        private Geometry3D GetDummyVisual()
        {
            return null;
        }
        /// <summary>
        /// Creates a new default configured factory
        /// </summary>
        /// <returns></returns>
        public static VisualFactory CreateDefaultFactory()
        {
            var providers = new Dictionary<RenderingMode, VisualProvider>();
            providers.Add(RenderingMode.Solid, new SolidMeshProvider());
            return new VisualFactory(providers);
        }
    }
}
