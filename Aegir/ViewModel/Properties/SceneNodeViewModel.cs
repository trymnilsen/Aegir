using AegirCore.Scene;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.ViewModel.Properties
{
    public class SceneNodeViewModel : NodeViewModel
    {
        private Transformation nodeTransform;

        [DisplayName("X")]
        [Category("World Transformation")]
        public double WorldTranslateX
        {
            get { return nodeTransform.Position.X; }
            set
            {
                double delta = value - nodeTransform.Position.X ;
                nodeTransform.TranslateX(delta);   
            }
        }
        [DisplayName("Y")]
        [Category("World Transformation")]
        public double WorldTranslateY
        {
            get { return nodeTransform.Position.Y; }
            set
            {
                double delta = value - nodeTransform.Position.Y;
                nodeTransform.TranslateY(delta);
            }
        }
        [DisplayName("Z")]
        [Category("World Transformation")]
        public double WorldTranslateZ
        {
            get { return nodeTransform.Position.Z; }
            set
            {
                double delta = value - nodeTransform.Position.Z;
                nodeTransform.TranslateZ(delta);
            }
        }
        public SceneNodeViewModel(SceneNode node)
           : base(node)
        {
            nodeTransform = node.Transform;
            nodeTransform.TransformationChanged += NodeTransform_TransformationChanged;
        }

        private void NodeTransform_TransformationChanged()
        {
            RaisePropertyChanged("WorldTranslateX");
            RaisePropertyChanged("WorldTranslateY");
            RaisePropertyChanged("WorldTranslate");
        }
    }
}
