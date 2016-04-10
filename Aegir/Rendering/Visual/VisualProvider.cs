using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public abstract class VisualProvider : IVisualProvider
    {
        protected Dictionary<RenderItem, Visual3D> visualCache;

        public VisualProvider()
        {
            visualCache = new Dictionary<RenderItem, Visual3D>();
        }

        public RenderItem GetRenderItem(Visual3D visual)
        {
            foreach(KeyValuePair<RenderItem,Visual3D> entry in visualCache)
            {
                if(entry.Value == visual)
                {
                    return entry.Key;
                }
            }
            return null;
        }

        public Visual3D GetVisual(RenderItem renderItem)
        {
            if (visualCache.ContainsKey(renderItem))
            {
                return visualCache[renderItem];
            }
            Visual3D visual = CreateVisual(renderItem);
            visualCache[renderItem] = visual;
            return visual;
        }

        protected abstract Visual3D CreateVisual(RenderItem renderItem);
    }
}