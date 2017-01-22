using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.EntityProxy.Node
{
    public class StaticNodeViewModel : SceneNodeViewModelBase
    {
        private string name;

        public StaticNodeViewModel(string name)
        {
            this.name = name;
        }

       
        [DisplayName("Name")]
        [Category("General")]
        public string Name
        {
            get
            {
                return name;
            }
        }

      
    }
}
