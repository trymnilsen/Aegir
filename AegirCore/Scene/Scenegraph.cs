using System.Collections.ObjectModel;

namespace AegirCore.Scene
{
    public class SceneGraph
    {
        public ObservableCollection<Node> RootNodes { get; set; }

        public SceneGraph()
        {
            RootNodes = new ObservableCollection<Node>();
        }

        public delegate void GraphChangedHandler();

        public event GraphChangedHandler GraphChanged;
    }
}