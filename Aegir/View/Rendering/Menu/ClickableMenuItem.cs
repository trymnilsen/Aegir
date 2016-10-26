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
        public bool Toggle { get; private set; }
        public RelayCommand ClickCommand { get; set; }

        public ClickableMenuItem(string header, Action command)
            : this(header, command, false) { }

        public ClickableMenuItem(string header, Action command, bool toggle)
        {
            this.Header = header;
            this.ClickCommand = new RelayCommand(command);
            this.Toggle = toggle;
        }
    }
}