using AegirLib.Output;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Message.Output
{
    /// <summary>
    /// Enum for simplifying if what happend to an output
    /// </summary>
    internal enum OutputChangedAction 
    {
        ADDED,
        REMOVED
    }
    internal class OutputChangedMessage
    {
        public Receiver Item { get; set; }
        public OutputChangedAction Action {get; set;}

        private OutputChangedMessage(Receiver item, OutputChangedAction action)
        {
            this.Item = item;
            this.Action = action;
        }
        public static void Send(Receiver newItem, OutputChangedAction action)
        {
            Messenger.Default.Send<OutputChangedMessage>(new OutputChangedMessage(newItem, action));
        }
    }
}
