using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.EntityProxy
{
    public abstract class SceneNodeViewModelBase : ViewModelBase, ISceneNode
    {
        private List<ISceneNode> children;
        private bool isEnabled;
        public List<ISceneNode> Children
        {
            get
            {
                return children;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                if(isEnabled!=value)
                {
                    isEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

    }
}
