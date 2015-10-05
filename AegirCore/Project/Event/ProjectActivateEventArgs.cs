using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Project.Event
{
    public class ProjectActivateEventArgs: EventArgs
    {
        public ProjectData Project { get; private set; }

        public ProjectActivateEventArgs(ProjectData project)
        {
            Project = project;
        }
    }
}
