using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public abstract class VisualProvider : IVisualProvider
    {
        protected Dictionary<SceneActor, Visual3D> visualCache;

        public VisualProvider()
        {
            visualCache = new Dictionary<SceneActor, Visual3D>();
        }

        public SceneActor GetRenderItem(Visual3D visual)
        {
            foreach (KeyValuePair<SceneActor, Visual3D> entry in visualCache)
            {
                if (entry.Value == visual)
                {
                    return entry.Key;
                }
            }
            return null;
        }

        public Visual3D GetVisual(SceneActor renderItem)
        {
            if (visualCache.ContainsKey(renderItem))
            {
                return visualCache[renderItem];
            }
            Visual3D visual = CreateVisual(renderItem);
            visualCache[renderItem] = visual;
            return visual;
        }

        protected abstract Visual3D CreateVisual(SceneActor renderItem);
    }
}