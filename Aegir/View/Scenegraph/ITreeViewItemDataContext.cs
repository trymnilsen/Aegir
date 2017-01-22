using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Scenegraph
{
    public interface ITreeViewItemDataContext
    {
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
    }
}
