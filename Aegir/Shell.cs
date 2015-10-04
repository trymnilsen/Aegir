using Aegir.Messages.Project;
using AegirCore;
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
            Messenger.Default.Register<LoadProjectFile>(this, OpenProject);
        }
        public void OpenProject(LoadProjectFile message)
        {
            Context.Project.OpenProject(message.FilePath);
        }
    }
}
