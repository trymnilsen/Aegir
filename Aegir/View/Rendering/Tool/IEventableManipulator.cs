using Aegir.View.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Rendering.Tool
{
    public delegate void TranslateFinishedHandler(ManipulatorFinishedEventArgs args);
    public interface IEventableManipulator
    {
        event TranslateFinishedHandler TranslateFinished;
    }
}
