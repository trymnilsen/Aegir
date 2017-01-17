using AegirLib.Behaviour.World;
using System.Collections.Generic;

namespace Aegir.Rendering
{
    public class GizmoHandler
    {
        public enum ViewportLayer
        {
            Overlay,
            Scene
        }

        private List<IGizmo> activeSceneGizmos;
        private bool visibility;

        public bool Visibility
        {
            get { return visibility; }
            set
            {
                if(value!=visibility)
                {
                    visibility = value;
                    GizmoVisibilityChanged?.Invoke(this, value);
                }
            }
        }

        public delegate void GizmoVisibilityChangedHandler(GizmoHandler sender, bool visibility);
        public event GizmoVisibilityChangedHandler GizmoVisibilityChanged;

        public static void AddGizmo(IGizmo gizmo, ViewportLayer layer)
        {
            GizmoAdded?.Invoke(gizmo);
        }
        private delegate void GizmoAddedHandler(IGizmo gizmo);
        private static event GizmoAddedHandler GizmoAdded; 
    }
}