namespace AegirCore.Behaviour.Rendering
{
    /// <summary>
    /// Contains data need to load the correct mesh to render
    /// </summary>
    public class RenderMeshBehaviour : BehaviourComponent
    {
        public string FilePath { get; private set; }

        public RenderMeshBehaviour(string filepath)
        {
            this.FilePath = filepath;
        }
    }
}