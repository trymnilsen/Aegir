using AegirLib.Project;
using GalaSoft.MvvmLight.Messaging;

namespace Aegir.Messages.Project
{
    public class ProjectActivated
    {
        public ProjectData Project { get; set; }

        private ProjectActivated(ProjectData project)
        {
            this.Project = project;
        }

        public static void Send(ProjectData project)
        {
            Messenger.Default.Send<ProjectActivated>(new ProjectActivated(project));
        }
    }
}