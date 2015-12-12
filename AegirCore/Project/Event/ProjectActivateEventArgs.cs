using System;

namespace AegirCore.Project.Event
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