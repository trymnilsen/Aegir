using System;

namespace AegirLib.Project.Event
{
    public class ProjectActivateEventArgs : EventArgs
    {
        public ProjectData Project { get; private set; }

        public ProjectActivateEventArgs(ProjectData project)
        {
            Project = project;
        }
    }
}