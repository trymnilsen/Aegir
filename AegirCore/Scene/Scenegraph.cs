using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Scene
{
    public class SceneGraph
    {
        public ObservableCollection<Node> RootNodes { get; set; }

        public SceneGraph()
        {
            RootNodes = new ObservableCollection<Node>();
        }
    }
}
