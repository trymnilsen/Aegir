using AegirLib.Behaviour.Simulation;
using AegirLib.Persistence;
using AegirLib.Project.Event;
using AegirLib.Scene;
using AegirLib.Vessel;
using System;
using System.IO;
using System.Xml.Serialization;

namespace AegirLib.Project
{
    //public class ProjectContext
    //{
    //    //private readonly ProjectPersister persistance;
    //    private ProjectData activeProject;

    //    public ProjectData ActiveProject
    //    {
    //        get
    //        {
    //            return activeProject;
    //        }
    //        private set
    //        {
    //            if (value != activeProject)
    //            {
    //                activeProject = value;
    //                TriggerActiveProject(value);
    //            }
    //        }
    //    }

    //    public ProjectContext()
    //    {
    //        //persistance = new ProjectPersister();
    //    }

    //    /// <summary>
    //    /// Opens a new project from a file and sets it as the active one
    //    /// </summary>
    //    /// <param name="filePath">The path to the .proj file</param>
    //    public void OpenProject(string filePath)
    //    {
    //        if (File.Exists(filePath))
    //        {
    //            ProjectLoadEventArgs args = new ProjectLoadEventArgs(filePath);
    //            TriggerProjectLoadFailure(args);
    //        }
    //    }

    //    public void OpenProject(ProjectData project)
    //    {
    //        ActiveProject = project;
    //    }

    //    private void TriggerProjectLoadSuccess(ProjectLoadEventArgs e)
    //    {
    //        ProjectLoadEventHandler evt = ProjectLoaded;
    //        if (evt != null)
    //        {
    //            evt(e);
    //        }
    //    }

    //    private void TriggerProjectLoadFailure(ProjectLoadEventArgs e)
    //    {
    //        ProjectLoadEventHandler evt = ProjectLoadFailure;
    //        if (evt != null)
    //        {
    //            evt(e);
    //        }
    //    }

    //    private void TriggerActiveProject(ProjectData project)
    //    {
    //        ProjectActivateEventHandler evt = ProjectActivated;
    //        if (evt != null)
    //        {
    //            evt(new ProjectActivateEventArgs(project));
    //        }
    //    }

    //    public delegate void ProjectLoadEventHandler(ProjectLoadEventArgs e);

    //    public delegate void ProjectActivateEventHandler(ProjectActivateEventArgs e);

    //    public event ProjectActivateEventHandler ProjectActivated;

    //    public event ProjectLoadEventHandler ProjectLoaded;

    //    public event ProjectLoadEventHandler ProjectLoadFailure;
    //}
}