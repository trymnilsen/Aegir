using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Behaviour.Vessel
{
    public class VesselNavigationBehaviour
    {
        private double speed;
        private double rateOfTurn;
        private double heading;

        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public double Heading
        {
            get { return heading; }
            set { heading = value; }
        }

        public double RateOfTurn
        {
            get { return rateOfTurn; }
            set { rateOfTurn = value; }
        }

    }
}
