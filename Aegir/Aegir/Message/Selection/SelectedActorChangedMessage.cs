using AegirLib.Data;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Message.Selection
{
    internal class SelectedActorChangedMessage
    {
        public Actor Item { get; set; }

        private SelectedActorChangedMessage(Actor item)
        {
            this.Item = item;
        }
        public static void Send(Actor newItem)
        {
            Messenger.Default.Send<SelectedActorChangedMessage>(new SelectedActorChangedMessage(newItem));
        }
    }
}
