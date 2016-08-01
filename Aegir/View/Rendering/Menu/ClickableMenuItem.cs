using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Rendering.Menu
{
    public class ClickableMenuItem : MenuListItem
    {
        public RelayCommand ClickCommand { get; set; }
        public ClickableMenuItem(string header, Action command)
        {
            this.Header = header;
            this.ClickCommand = new RelayCommand(command);
        }
    }
}
