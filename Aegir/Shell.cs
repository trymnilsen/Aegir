using Aegir.Messages.Project;
using Aegir.Messages.Simulation;
using Aegir.Messages.Timeline;
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
            ProjectData newProject = Context.Project.CreateNewProject();
            Context.Project.OpenProject(newProject);
            ActiveTimelineChanged.Send(Context.Engine.KeyframeEngine.Keyframes, Context.Engine.KeyframeEngine);
        }

        private void OpenProject(LoadProjectFile message)
        {
            Context.Project.OpenProject(message.FilePath);
        }
        private void SaveProject(SaveProjectFile message)
        {
            Context.Project.SaveProject(message.FilePath);
        }
        private void OnProjectActivated(ProjectActivateEventArgs e)
        {
            ProjectActivated.Send(e.Project);
        }
    }
}