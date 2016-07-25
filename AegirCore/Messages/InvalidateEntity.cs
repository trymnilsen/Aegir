using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace AegirCore.Messages
{
    public class InvalidateEntity : GenericTinyMessage<Node>
    {
        public InvalidateEntity(object sender, Node content) 
            : base(sender, content) { }

    }
}
