using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel
{
    public class SelectionViewModel : ViewModelBase
    {
        private object selectedObject;

        public object SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                RaisePropertyChanged();
            }
        }

    }
}
