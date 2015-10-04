using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Project
{
    public class ProjectContext
    {
        public ProjectData ActiveProject { get; private set; }
        public void OpenProject(string filePath)
        {
            if(File.Exists(filePath))
            {
                ProjectLoadEventArgs args = new ProjectLoadEventArgs(filePath);
                TriggerProjectLoadFailure(args);
            }
        }

        private void TriggerProjectLoadSuccess(ProjectLoadEventArgs e)
        {
            ProjectLoadEventHandler evt = ProjectLoaded;
            if(evt!=null)
            {
                evt(e);
            }
        }
        private void TriggerProjectLoadFailure(ProjectLoadEventArgs e)
        {
            ProjectLoadEventHandler evt = ProjectLoadFailure;
            if (evt != null)
            {
                evt(e);
            }
        }

        public delegate void ProjectLoadEventHandler(ProjectLoadEventArgs e);

        public event ProjectLoadEventHandler ProjectLoaded;
        public event ProjectLoadEventHandler ProjectLoadFailure;

    }
}
