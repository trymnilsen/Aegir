namespace AegirLib.Mesh.Loader
{
    public interface IModelLoader
    {
        MeshData LoadModel(string FileContent);
    }
}