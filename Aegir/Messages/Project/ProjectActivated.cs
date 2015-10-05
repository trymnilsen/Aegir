using AegirCore.Project;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
