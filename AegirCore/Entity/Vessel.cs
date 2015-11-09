using AegirCore.Scene;
using AegirCore.Simulation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace AegirCore.Entity
{
    public class Vessel : SceneNode
    {
        [Category("Simulation")]
        [DisplayName("Simulation Mode")]
        public VesselSimulationMode SimulationMode { get; set; }
        public Vessel()
        {
            this.Name = "Vessel";
            ModelPath = "Content/ship.obj";
        }

        public override void Update(SimulationTime deltaTime)
        {
            if(SimulationMode == VesselSimulationMode.Simulate)
            {
                UpdateSimulation(deltaTime);
            }
            base.Update(deltaTime);
        }
        private void UpdateSimulation(SimulationTime time)
        {
            //TODO: MOVE TO BEHAVIOUR
            ////Rate of turn is in degrees minutes, let's convert it
            //double stepRot = RateOfTurn / (60 * 30);
            //double newHeading = Heading + stepRot;
            //Vector3D newMovement = new Vector3D(Math.Cos((Math.PI / 180) * (newHeading - 90)) * Speed, -Math.Sin((Math.PI / 180) * (newHeading-90))* Speed, 0);
            //Vector3D transformPos = Transform.Position;
            //Vector3D newPosition = transformPos + newMovement;
            //Debug.WriteLine("Speed: " + Speed + " New Pos:" + newPosition.X + " / " + newPosition.Y);
            //Transform.Position = Transform.Position + newMovement;
            ////Transform.RotateHeading(newHeading);
            //Heading = newHeading;
        }
    }
}
