using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Statusbar
{
    public class Operation : ObservableObject
    {

        private double maxProgress;
        private double minProgress;
        private double curProgress;

        public string Name { get; private set; }
        public bool Indeterminate { get; private set; }
        private bool finished;

        public bool IsFinished
        {
            get { return finished; }
            set
            {
                if(finished!=value)
                {
                    finished = value;
                    OperationFinished?.Invoke(this);
                }
            }
        }

        public double MinimumProgress
        {
            get { return minProgress; }
            set
            {
                if (value != minProgress)
                {
                    minProgress = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MaximumProgress
        {
            get { return maxProgress; }
            set
            {
                if (value != maxProgress)
                {
                    maxProgress = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double CurrentProgress
        {
            get { return curProgress; }
            set
            {
                if (value != curProgress)
                {
                    curProgress = value;
                    RaisePropertyChanged();
                }
            }
        }
        
        public Operation(string name, bool indeterminate)
        {
            Name = name;
            Indeterminate = indeterminate;
        }
        
        public event OperationFinishedHandler OperationFinished;
        public delegate void OperationFinishedHandler(Operation op);
    }
}
