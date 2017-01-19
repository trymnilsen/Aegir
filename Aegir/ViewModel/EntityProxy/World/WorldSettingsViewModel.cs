using AegirLib.Behaviour.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.EntityProxy.World
{
    [ViewModelForBehaviour(typeof(WorldSettings))]
    [DisplayName("World Settings")]
    public class WorldSettingsViewModel : TypedBehaviourViewModel<WorldSettings>
    {
        public int Year { get; set; } = 2016;
        public int Month { get; set; } = 11;
        public int Day { get; set; } = 03;
        public double Hour { get; set; } = 09;
        public double Minute { get; set; } = 17;
        public double Second { get; set; } = 43;

        [DisplayName("Sun Azimuth")]
        public double SunAzimuth { get; set; } = 232;

        [DisplayName("Sun Altitude")]
        public double SunAltitude { get; set; } = 16;

        public WorldSettingsViewModel(WorldSettings component)
            : base(component)
        {
        }

        internal override void Invalidate()
        {
        }
    }
}