using AegirCore.Entity;
using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy
{
    public class GeoidViewModelProxy : NodeViewModelProxy
    {
        private double geoidHeightOffset;
        [Category("Geodesy")]
        [DisplayName("Geoid Height Offset")]
        public double GeoidHeightOffset
        {
            get { return geoidHeightOffset; }
            set { geoidHeightOffset = value; }
        }

        public GeoidViewModelProxy(Geoid geoid, IScenegraphAddRemoveHandler removeHandler) : base(geoid, removeHandler)
        {

        }
    }
}
