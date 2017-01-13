using Aegir.Rendering.Visual;

namespace Aegir.Rendering
{
    public interface IRenderViewport
    {
        void ChangeRenderingMode(RenderingMode mode);
        void RenderActor(SceneActor actor);
        void ClearView();
        void InvalidateActors();
        void RemoveActor(SceneActor actor);
        GizmoHandler SceneGizmos { get; }
        VisualFactory VisualFactory { get; set; }
    }
}