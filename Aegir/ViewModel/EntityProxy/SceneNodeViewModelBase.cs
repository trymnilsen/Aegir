using Aegir.Mvvm;
using Aegir.View.Scenegraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.EntityProxy
{
    public abstract class SceneNodeViewModelBase : ViewModelBase, ISceneNode, 
                                                                  ITreeViewItemDataContext
    {
        private List<ISceneNode> children = new List<ISceneNode>();
        private bool isEnabled = true;
        [Browsable(false)]
        public List<ISceneNode> Children
        {
            get
            {
                return children;
            }
        }

        [DisplayName("Is Enabled")]
        [Category("General")]
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

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }
    }
}
