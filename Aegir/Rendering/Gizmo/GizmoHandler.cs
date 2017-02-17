using Aegir.Rendering.Gizmo;
using AegirLib.Behaviour.World;
using AegirLib.Scene;
using System.Collections.Generic;

namespace Aegir.Rendering.Gizmo
{
    public class GizmoHandler
    {
        public enum ViewportLayer
        {
            Overlay,
            Scene
        }

        private List<IGizmo> sceneGizmos;

        public GizmoHandler()
        {
            sceneGizmos = new List<IGizmo>();

            //Add gizmos

            sceneGizmos.Add(new TranslateGizmo());
        }
        public void SelectionChanged(Entity entity)
        {
            foreach(IGizmo gizmo in sceneGizmos)
            {
                gizmo.TargetNode = entity;
                if(gizmo.GizmoIsVisible)
                {
                    SelectionGizmoAdded?.Invoke(gizmo, gizmo.Layer);
                }
                else
                {
                    SelectionGizmoRemoved?.Invoke(gizmo, gizmo.Layer);
                }
            }
        }

        public delegate void GizmoChangeHandler(IGizmo gizmo, ViewportLayer layer);
        public event GizmoChangeHandler SelectionGizmoAdded;
        public event GizmoChangeHandler SelectionGizmoRemoved;

        //public delegate void GizmoVisibilityChangedHandler(GizmoHandler sender, bool visibility);
        //public event GizmoVisibilityChangedHandler GizmoVisibilityChanged;

        //public static void AddGizmo(IGizmo gizmo, ViewportLayer layer)
        //{
        //    GizmoAdded?.Invoke(gizmo);
        //}

    }
}