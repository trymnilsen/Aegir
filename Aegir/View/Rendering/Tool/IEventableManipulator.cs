using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Rendering.Tool
{
    public interface IEventableManipulator
    {
        public delegate void TranslateFinishedHandler(TranslateFinishedEventArgs args);
        public event TranslateFinishedHandler TranslateFinished;
    }
}
