using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.EntityProxy.Node
{
    public class GizmoNodeViewModel : SceneNodeViewModelBase
    {
        public GizmoNodeViewModel(string name)
        {

        }
        [DisplayName("Name")]
        [Category("General")]
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
