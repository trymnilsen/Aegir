using AegirCore.Scene;
using AegirCore.Vessel;
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
        public ProjectContext()
        {
            
        }
        /// <summary>
        /// Opens a new project from a file and sets it as the active one
        /// </summary>
        /// <param name="filePath">The path to the .proj file</param>
        public void OpenProject(string filePath)
        {
            if(File.Exists(filePath))
            {
                ProjectLoadEventArgs args = new ProjectLoadEventArgs(filePath);
                TriggerProjectLoadFailure(args);
            }
        }

        public ProjectData CreateNewProject()
        {
            SceneGraph scene = new SceneGraph();
            VesselConfiguration vessel = new VesselConfiguration();

            return new ProjectData(scene, vessel, "New Simulation");
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
