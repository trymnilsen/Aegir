using AegirLib.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace AegirLib.Messages
{
    public class InvalidateEntity : GenericTinyMessage<Entity>
    {
        public InvalidateEntity(object sender, Entity content)
            : base(sender, content) { }
    }
}