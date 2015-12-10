using AegirCore.Behaviour.Simulation;
using AegirCore.Behaviour.Vessel;
using AegirCore.Entity;
using AegirCore.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Aegir.ViewModel.NodeProxy
{
    public class VesselViewModelProxy : NodeViewModelProxy
    {
        private VesselNavigationBehaviour navBehaviour;
        private FloatingMesh floatMeshBehaviour;
        [Category("Motion")]
        public double Heading
        {
            get
            {
                return navBehaviour.Heading * (180 / Math.PI);
            }
            set
            {
                navBehaviour.Heading = value * (Math.PI / 180);
                RaisePropertyChanged();
            }
        }

        [Category("Motion")]
        [Editor(typeof(PropertyGridEditorDecimalUpDown),typeof(PropertyGridEditorDecimalUpDown))]
        public double Speed
        {
            get { return navBehaviour.Speed; }
            set
            {
                navBehaviour.Speed = value;
                RaisePropertyChanged();
            }
        }
        [Category("Motion")]
        [DisplayName("Rate Of Turn")]
        public double RateOfTurn
        {
            get { return navBehaviour.RateOfTurn; }
            set
            {
                navBehaviour.RateOfTurn = value;
                RaisePropertyChanged();
            }
        }
        [Category("Simulation")]
        [DisplayName("Simulation Mode")]
        public VesselSimulationMode SimMode
        {
            get { return navBehaviour.SimulationMode; }
            set
            {
                navBehaviour.SimulationMode = value;
                RaisePropertyChanged();
            }
        }
        [Category("Simulation")]
        public float Mass
       { 
            get { return floatMeshBehaviour.Mass; }
            set { floatMeshBehaviour.Mass = value; }
        }

        [Category("Simulation")]
        [DisplayName("Hull Model")]
        public string VesselHull
        {
            get { return floatMeshBehaviour.HullModelPath; }
            set
            {
                floatMeshBehaviour.HullModelPath = value;
                RaisePropertyChanged();
            }
        }
        public VesselViewModelProxy(Vessel vessel)
            :base(vessel)
        {
            navBehaviour = vessel.GetComponent<VesselNavigationBehaviour>();
            floatMeshBehaviour = vessel.GetComponent<FloatingMesh>();
        }
        public override void Invalidate()
        {
            RaisePropertyChanged(nameof(RateOfTurn));
            RaisePropertyChanged(nameof(Heading));
            RaisePropertyChanged(nameof(Speed));
            base.Invalidate();
        }
    }
}
