using Aegir.Rendering.Visual;
using AegirLib.Behaviour.World;

namespace Aegir.Rendering
{
    public interface IRenderViewport
    {
        void ChangeRenderingMode(RenderingMode mode);
        void RenderActor(SceneActor actor);
        void RenderDummy(Transform transform);
        void ClearView();
        void InvalidateActors();
        void RemoveActor(SceneActor actor);
        GizmoHandler SceneGizmos { get; }
        VisualFactory VisualFactory { get; set; }
    }
}