﻿using Aegir.Messages.Project;
using Aegir.Messages.Simulation;
using AegirCore;
using AegirCore.Project;
using AegirCore.Project.Event;
using GalaSoft.MvvmLight.Messaging;

namespace Aegir
{
    public class Shell
    {
        public ApplicationContext Context { get; private set; }

        public Shell()
        {
            /*Environment*/
            Context = new ApplicationContext();
            Messenger.Default.Register<LoadProjectFile>(this, OpenProject);

            Context.Project.ProjectActivated += OnProjectActivated;
            Context.Engine.StepFinished += Engine_StepFinished;
        }

        private void Engine_StepFinished()
        {
            InvalidateEntities.Send();
        }

        public void ShellLoaded()
        {
            ProjectData newProject = Context.Project.CreateNewProject();
            Context.Project.OpenProject(newProject);
        }

        private void OpenProject(LoadProjectFile message)
        {
            Context.Project.OpenProject(message.FilePath);
        }

        private void OnProjectActivated(ProjectActivateEventArgs e)
        {
            ProjectActivated.Send(e.Project);
        }
    }
}