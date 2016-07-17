using Aegir.Messages.Project;
using Aegir.Messages.Simulation;
using Aegir.Messages.Timeline;
using AegirCore;
using AegirCore.Project;
using AegirCore.Project.Event;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;

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
            Messenger.Default.Register<SaveProjectFile>(this, SaveProject);

            Context.Project.ProjectActivated += OnProjectActivated;
            Context.Engine.StepFinished += Engine_StepFinished;
        }

        private void Engine_StepFinished()
        {
            InvalidateEntities.Send();
        }

        public void ShellLoaded()
       {

            //ProjectData newProject = Context.Project.CreateNewProject();
            //Context.Project.OpenProject(newProject);
            //ActiveTimelineChanged.Send(Context.Engine.KeyframeEngine.Keyframes, Context.Engine.KeyframeEngine);
            try
            {
                Context.SaveLoadHandler.LoadDefault();
            }
            catch(Exception e)
            {
                MessageBox.Show("Error Occured while setting up default project:\n" + e.Message + "\n\n" + e.StackTrace);
            }
        }

        private void OpenProject(LoadProjectFile message)
        {
            Context.SaveLoadHandler.LoadState(message.FilePath);
        }
        private void SaveProject(SaveProjectFile message)
        {
            Context.SaveLoadHandler.SaveState(message.FilePath);
        }
        private void OnProjectActivated(ProjectActivateEventArgs e)
        {
            ProjectActivated.Send(e.Project);
        }
    }
}