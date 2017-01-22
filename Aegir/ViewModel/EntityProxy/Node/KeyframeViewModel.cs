using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.EntityProxy.Node
{
    public class KeyframeViewModel : SceneNodeViewModelBase
    {
        private int Time { get; set; }

        public KeyframeViewModel(int time)
        {
            this.Time = time;
        }

       
        [DisplayName("Name")]
        [Category("General")]
        public string Name
        {
            get
            {
                return $"Key {Time}";
            }
        }
    }
}
