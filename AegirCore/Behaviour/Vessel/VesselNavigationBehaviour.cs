using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirCore.Simulation;
using OpenTK;
using AegirCore.Behaviour.World;
using System.Diagnostics;

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
        public override void Update(SimulationTime time)
        {
            if(simMode == VesselSimulationMode.Simulate)
            {
                UpdateSimulation(time);
            }
            base.Update(time);
        }
        public void UpdateSimulation(SimulationTime time)
        {
            if(transform== null)
            {
                transform = GetComponent<TransformBehaviour>();
            }
            //Rate of turn is in degrees minutes, let's convert it to radians per update
            double rotRads = rateOfTurn * (Math.PI / 180);
            double stepRot = rotRads / (60 * time.TrueUpdatePerSecond);
            double newHeading = Heading + stepRot;
            Vector3d newMovement = new Vector3d(Math.Cos(newHeading + Math.PI / 2) * Speed, Math.Sin(newHeading + Math.PI / 2) * Speed, 0);
            Vector3d transformPos = transform.Position;
            Vector3d newPosition = transformPos + newMovement;
            Debug.WriteLine("Speed: " + Speed + " New Pos:" + newPosition.X + " / " + newPosition.Y);
            transform.Position = transform.Position + newMovement;
            transform.RotateHeading(newHeading);
            Heading = newHeading;
        }

    }
}
