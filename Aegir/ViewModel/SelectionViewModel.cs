using Aegir.Messages.Selection;
using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Aegir.ViewModel
{
    public class SelectionViewModel : ViewModelBase
    {
        private object selectedObject;

        public SelectionViewModel(ITinyMessengerHub messageHub)
        {
            Messenger = messageHub;
            Messenger.Subscribe<SelectionChanged>(OnSelectionChanged);
        }

        private void OnSelectionChanged(SelectionChanged obj)
        {
            SelectedObject = obj.Content;
        }

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
