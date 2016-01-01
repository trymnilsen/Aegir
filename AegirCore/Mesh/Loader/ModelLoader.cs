namespace AegirCore.Mesh.Loader
{
    public interface ModelLoader
    {
        IndexedMeshData LoadModel(string FileContent);
    }
}