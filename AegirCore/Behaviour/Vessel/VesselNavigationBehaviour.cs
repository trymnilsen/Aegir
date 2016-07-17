using AegirCore.Behaviour.World;
using AegirCore.Persistence;
using AegirCore.Scene;
using AegirCore.Simulation;
using AegirType;
using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace AegirCore.Behaviour.Vessel
{
    public class VesselNavigationBehaviour : BehaviourComponent
    {
        private double speed;
        private double rateOfTurn;
        private double heading;
        private TransformBehaviour transform;
        private VesselSimulationMode simMode;

        public VesselSimulationMode SimulationMode
        {
            get { return simMode; }
            set { simMode = value; }
        }

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
        public VesselNavigationBehaviour(Node parentNode)
            :base()
        {

        }
        public override void Update(SimulationTime time)
        {
            if (simMode == VesselSimulationMode.Simulate)
            {
                UpdateSimulation(time);
            }
            base.Update(time);
        }

        public void UpdateSimulation(SimulationTime time)
        {
            if (transform == null)
            {
                transform = GetComponent<TransformBehaviour>();
            }
            //Rate of turn is in degrees minutes, let's convert it to radians per update
            double rotRads = rateOfTurn * (Math.PI / 180);
            double stepRot = rotRads / (60 * time.TrueUpdatePerSecond);
            double newHeading = Heading + stepRot;
            Vector3 newMovement = new Vector3((float)Math.Cos(newHeading + Math.PI / 2) * (float)Speed, (float)Math.Sin(newHeading + Math.PI / 2) * (float)Speed, 0);
            Vector3 transformPos = transform.Position;
            Vector3 newPosition = transformPos + newMovement;
            Debug.WriteLine("Speed: " + Speed + " New Pos:" + newPosition.X + " / " + newPosition.Y);
            transform.Position = transform.Position + newMovement;
            transform.RotateHeading(newHeading);
            Heading = newHeading;
        }

        public override XElement Serialize()
        {
            XElement element = new XElement(this.GetType().Name);
            element.AddElement(nameof(Speed),Speed);
            element.AddElement(nameof(RateOfTurn), RateOfTurn);
            element.AddElement(nameof(Heading), Heading);
            element.AddElement(nameof(SimulationMode), SimulationMode);

            return element;
        }

        public override void Deserialize(XElement data)
        {
            Speed = data.GetElementAs<double>(nameof(speed));
            Heading = data.GetElementAs<double>(nameof(speed));
            RateOfTurn = data.GetElementAs<double>(nameof(speed));
            SimulationMode = data.GetElementAs<VesselSimulationMode>(nameof(SimulationMode));

        }
    }
}