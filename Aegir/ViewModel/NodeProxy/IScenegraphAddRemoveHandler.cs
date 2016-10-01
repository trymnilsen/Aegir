namespace Aegir.ViewModel.NodeProxy
{
    public interface IScenegraphAddRemoveHandler
    {
        void Remove(NodeViewModelProxy nodeViewModelProxy);
        void Add(string type);
    }
}