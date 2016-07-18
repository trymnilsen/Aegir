using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Messenger
{
    public class MessageSubscriptionToken : IDisposable
    {
        private MessageHub messageHub;

        public MessageSubscriptionToken(MessageHub messageHub)
        {
            this.messageHub = messageHub;
        }
        public void Dispose()
        {
            this.messageHub.Unsubscribe(this);
        }
    }
}
