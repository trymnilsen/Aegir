using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AegirViewHelper
{
    public interface IMenuItem
    {
        /// <summary>
        /// The text to be shown on Item
        /// </summary>
        string Text { get; }
        /// <summary>
        /// Where in the menu tree to put this menu item
        /// Position is given by defining the parent items text separated by dot
        /// E.G "file.options" Will put this item under options under file
        /// </summary>
        string Placement { get; }
        /// <summary>
        /// Command to execute on click
        /// </summary>
        ICommand Command {get; }
    }
}
