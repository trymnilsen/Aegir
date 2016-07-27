using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aegir.View.Rendering.Tool
{
    public interface IMouseDownManipulator
    {
        void RaiseMouseDown(MouseButtonEventArgs e);
        void RaiseMouseUp(MouseButtonEventArgs e);
    }
}
