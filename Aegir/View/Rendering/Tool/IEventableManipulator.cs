using Aegir.View.Rendering.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Rendering.Tool
{
    public delegate void ManipulationFinishedHandler(ManipulatorFinishedEventArgs args);
    public interface IEventableManipulator
    {
        event ManipulationFinishedHandler ManipulationFinished;
    }
}
