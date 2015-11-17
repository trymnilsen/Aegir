using AegirCore.Behaviour.Vessel;
using AegirCore.Entity;
using AegirCore.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy
{
    public class VesselViewModelProxy : NodeViewModelProxy
    {
        private VesselNavigationBehaviour navBehaviour;

        [Category("Motion")]
        public double Heading
        {
            get { return navBehaviour.Heading; }
            set
            {
                navBehaviour.Heading = value;
                RaisePropertyChanged();
            }
        }

        [Category("Motion")]
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
        public VesselViewModelProxy(Vessel vessel)
            :base(vessel)
        {

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
