namespace Aegir.ViewModel.NodeProxy
{
    public interface IScenegraphAddRemoveHandler
    {
        void Remove(NodeViewModel nodeViewModelProxy);
        void Add(string type);
    }
}