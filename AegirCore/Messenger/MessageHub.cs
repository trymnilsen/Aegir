using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Messenger
{
    public class MessageHub
    {
        public MessageSubscriptionToken Subscribe<T>(Action<T> receipient) where T : class
        {
            throw new NotImplementedException();
        }

        internal void Unsubscribe(MessageSubscriptionToken messageSubscriptionToken)
        {
            throw new NotImplementedException();
        }
    }
}
