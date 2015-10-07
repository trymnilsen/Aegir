using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Scene
{
    public class Node
    {
        public string Name {get; set;}
        public bool Nestable { get; set; }
        public bool Removable { get; set; }
        public ObservableCollection<Node> Children { get; set; }

        public Node()
        {
            Children = new ObservableCollection<Node>();
        }
    }
}
