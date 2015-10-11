using AegirCore.Behaviour;
using AegirCore.Scene;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Aegir.ViewModel.Properties
{
    public class NodeViewModel : ViewModelBase
    {
        private Node activeNode;
        //[ExpandableObject]
        [TypeConverter(typeof(ComponentCollectionConverter))]
        public ComponentViewModelCollection Components { get; set; }
        
        public NodeViewModel(Node node)
        {
            this.activeNode = node;
            PopulateComponents();
        }
        private void PopulateComponents()
        {
            if (Components == null)
            {
                Components = new ComponentViewModelCollection();
            }
            if (Components.Count > 0)
            {
                Components.Clear();
            }

            foreach (BehaviourComponent component in activeNode.Components)
            {
                Components.Add(new ComponentViewModel(component));
            }
        }
    }
    public class Foo 
    {
        public string SomeProp { get; set; }
        //public Bar someBarPorp { get; set; }
    }
    public class Bar
    {
        public int BarValue { get; set; }
        public string GotHereText { get; set; }
    }
}
