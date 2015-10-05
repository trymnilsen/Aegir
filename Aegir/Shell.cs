using Aegir.Messages.Project;
using AegirCore;
using AegirCore.Project.Event;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir
{
    public class Shell
    {
        public ApplicationContext Context { get; private set; }
        public Shell()
        {
            Context = new ApplicationContext();
            Messenger.Default.Register<LoadProjectFile>(this, OpenProject);

            Context.Project.ProjectActivated += OnProjectActivated;
        }
        public void OpenProject(LoadProjectFile message)
        {
            Context.Project.OpenProject(message.FilePath);
        }
        public void OnProjectActivated(ProjectActivateEventArgs e)
        {
            ProjectActivated.Send(e.Project);
        }
    }
}
