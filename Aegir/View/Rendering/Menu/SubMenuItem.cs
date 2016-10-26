using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Rendering.Menu
{
    public class SubMenuItem : MenuListItem
    {
        public MenuListItem[] Children { get; private set; }

        public SubMenuItem(string header, MenuListItem[] children)
        {
            this.Header = header;
            this.Children = children;
        }
    }
}