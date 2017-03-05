using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AegirLib.Scene;

namespace AegirLib.Behaviour.Output
{
    public class UDPOutput : BehaviourComponent
    {
        public UDPOutput(Entity parent) : base(parent)
        {
        }

        public override void Deserialize(XElement data)
        {
            throw new NotImplementedException();
        }

        public override XElement Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
