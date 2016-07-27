using Aegir.View.Rendering.Tool;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aegir.View.Rendering.Tool
{
    class EventableBindableRotateManipulator : BindableRotateManipulator, IEventableManipulator, IMouseDownManipulator
    {
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            TriggerTranslateFinished();
        }

        private void TriggerTranslateFinished()
        {
            ManipulationFinishedHandler evt = ManipulationFinished;
            if (evt != null)
            {
                evt(new ManipulatorFinishedEventArgs());
            }
        }

        public void RaiseMouseDown(MouseButtonEventArgs e)
        {
            OnMouseDown(e);
        }

        public event ManipulationFinishedHandler ManipulationFinished;
    }
}
