using Aegir.Messages.Project;
using Aegir.Messages.Simulation;
using AegirCore.Scene;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy
{
    public class ScenegraphViewModelProxy : ViewModelBase
    {
        private SceneGraph sceneSource;
        public ObservableCollection<NodeViewModelProxy> Items { get; set; }
        public ScenegraphViewModelProxy()
        {
            MessengerInstance.Register<InvalidateEntities>(this, OnInvalidateEntitiesMessage);
            MessengerInstance.Register<ProjectActivated>(this, OnProjectActivated);
        }
        public void OnInvalidateEntitiesMessage(InvalidateEntities message)
        {

        }
        public void OnProjectActivated(ProjectActivated message)
        {
            if(sceneSource!=null)
            {
                sceneSource.GraphChanged -= SceneSource_GraphChanged;
            }
            sceneSource = message.Project.Scene;
            sceneSource.GraphChanged += SceneSource_GraphChanged;
        }

        private void SceneSource_GraphChanged()
        {
            //At the moment we rebuild the graph if it's changed
            TriggerScenegraphChanged();
        }
        public void TriggerInvalidateChildren()
        {
            InvalidateChildrenHandler InvalidateEvent = InvalidateChildren;
            if (InvalidateEvent != null)
            {
                InvalidateEvent();
            }
        }

        public void TriggerScenegraphChanged()
        {
            ScenegraphChangedHandler ChangeEvent = ScenegraphChanged;
            if (ChangeEvent != null)
            {
                ChangeEvent();
            }
        }

        public delegate void InvalidateChildrenHandler();
        public event InvalidateChildrenHandler InvalidateChildren;

        public delegate void ScenegraphChangedHandler();
        public event ScenegraphChangedHandler ScenegraphChanged;
    }
}
