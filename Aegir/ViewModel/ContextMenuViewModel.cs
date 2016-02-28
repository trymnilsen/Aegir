using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aegir.ViewModel
{
    public class ContextMenuViewModel : ViewModelBase
    {
        private ICommand command;

        public ICommand Command
        {
            get { return command; }
            set
            { 
                command = value;
                RaisePropertyChanged();
            }
        }

        private string name;
                
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }
        public ContextMenuViewModel(string name, ICommand command)
        {
            this.command = command;
            this.name = name;
        }


    }
}
