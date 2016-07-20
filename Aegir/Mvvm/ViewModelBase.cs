using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Aegir.Mvvm
{
    public class ViewModelBase : ObservableObject
    {
        public ITinyMessengerHub Messenger { get; private set; }
        public ViewModelBase(ITinyMessengerHub messenger)
        {
            Messenger = messenger;
        }
    }
}
