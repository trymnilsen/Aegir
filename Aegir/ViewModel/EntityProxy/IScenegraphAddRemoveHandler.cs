namespace Aegir.ViewModel.EntityProxy
{
    public interface IScenegraphAddRemoveHandler
    {
        void Remove(EntityViewModel entityViewModelProxy);

        void Add(string type);
    }
}