using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aegir.View.Tool
{
    public class EventableBindableTranslateManipulator : BindableTranslateManipulator
    {
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            TriggerTranslateFinished();
        }

        private void TriggerTranslateFinished()
        {
            TranslateFinishedHandler evt = TranslateFinished;
            if (evt != null)
            {
                TranslateFinished(new TranslateFinishedEventArgs());
            }
        }
        public event TranslateFinishedHandler TranslateFinished;

    }
}
