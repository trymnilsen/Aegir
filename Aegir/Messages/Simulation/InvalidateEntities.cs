using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Messages.Simulation
{
    public class InvalidateEntities
    {

        private InvalidateEntities()
        {
        }

        public static void Send()
        {
            Messenger.Default.Send<InvalidateEntities>(new InvalidateEntities());
        }
    }
}
