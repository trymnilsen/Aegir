using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AegirLib.Scene;
using AegirLib.Persistence;

namespace AegirLib.Behaviour.Vessel
{
    public class VesselDimentionsBehaviour : BehaviourComponent
    {
        private double width;

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        private double length;

        public double Length
        {
            get { return length; }
            set { length = value; }
        }

        private double height;

        public double Height
        {
            get { return height; }
            set { height = value; }
        }


        public VesselDimentionsBehaviour(Entity parent) : base(parent)
        {

        }

        public override XElement Serialize()
        {
            XElement element = new XElement(this.GetType().Name);
            element.AddElement(nameof(Width), Width);
            element.AddElement(nameof(Length), Length);
            element.AddElement(nameof(Height), Height);

            return element;
        }

        public override void Deserialize(XElement data)
        {
            Width = data.GetElementAs<double>(nameof(Width));
            Length = data.GetElementAs<double>(nameof(Length));
            Height = data.GetElementAs<double>(nameof(Height));
        }
    }
}
