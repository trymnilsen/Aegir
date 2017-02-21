using Aegir.Rendering.Gizmo;
using Aegir.Rendering.Gizmo.Transform;
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
            sceneGizmos.Add(new ManipulatorGizmo());
            //sceneGizmos.Add(new TranslateGizmo());
        }
        public void SelectionChanged(ITransformableVisual visual)
        {
            foreach(IGizmo gizmo in sceneGizmos)
            {
                bool isGizmoVisible = gizmo.UpdateGizmoSelection(visual);
                if (isGizmoVisible)
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