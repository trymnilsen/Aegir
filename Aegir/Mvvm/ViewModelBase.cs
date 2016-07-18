using AegirCore.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Mvvm
{
    public class ViewModelBase : ObservableObject
    {
        public MessageHub Messenger { get; private set; }
        public ViewModelBase(MessageHub messenger)
        {
            Messenger = messenger;
        }
    }
}
