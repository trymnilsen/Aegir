using Aegir.Messages.ObjectTree;
using Aegir.Messages.Timeline;
using AegirCore.Keyframe;
using AegirCore.Scene;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Timeline
{
    /// <summary>
    /// Viewmodel for timelineviewmodel
    /// </summary>
    public class TimelineViewModel : ViewModelBase
    {
        /// <summary>
        /// Backing store for if the timeline is scoped
        /// </summary>
        private bool isScoped;

        /// <summary>
        /// Backingstore for the timeline
        /// </summary>
        private KeyframeTimeline timeline;

        /// <summary>
        /// the currently used active node
        /// </summary>
        private Node activeNode;

        private int timelineStart;
        private int timelineEnd;
        private int currentTimelinePosition;
        /// <summary>
        /// Command for creating keyframes at the current timeline 
        /// value for the selected node
        /// </summary>
        public RelayCommand AddKeyframeCommand { get; set; }
        /// <summary>
        /// The timeline we currently are using and reading/adding keyframes to
        /// </summary>
        public KeyframeTimeline Timeline
        {
            get { return timeline; }
            set
            {
                //Set the timeline
                SetTimeLine(value);
                //Notify that we have changed it
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Collection of viewmodels for the keyframes on our current timeline
        /// </summary>
        public ObservableCollection<KeyframeViewModel> Keyframes { get; private set; }
        /// <summary>
        /// Is the timeline currently scoped to our active object or active for all
        /// </summary>
        public bool IsScopedToNode
        {
            get { return isScoped; }
            set { isScoped = value; }
        }
        /// <summary>
        /// Current position on our timeline
        /// </summary>
        public int Time
        {
            get { return currentTimelinePosition; }
            set { currentTimelinePosition = value; }    
        }

        /// <summary>
        /// Where our timeline starts
        /// </summary>
        public int TimelineStart
        {
            get { return timelineStart; }
            set { timelineStart = value; }
        }

        /// <summary>
        /// Where our timeline ends
        /// </summary>
        public int TimelineEnd
        {
            get { return timelineEnd; }
            set { timelineEnd = value; }
        }

        /// <summary>
        /// Instantiates a new timeline viewmodel
        /// </summary>
        public TimelineViewModel()
        {
            MessengerInstance.Register<SelectedNodeChanged>(this, ActiveNodeChanged);
            MessengerInstance.Register<ActiveTimelineChanged>(this, TimelineChanged);
            AddKeyframeCommand = new RelayCommand(AddKeyframes);
        }
        /// <summary>
        /// Requests the timeline to create a new "Snapshot" of keyframes for the given timeline time
        /// </summary>
        private void AddKeyframes()
        {
            timeline.CreateKeyframeOnNode(activeNode, Time);
        }
        /// <summary>
        /// Message callback for timeline instance changed
        /// </summary>
        /// <param name="message">A message containing the new timeline</param>
        private void TimelineChanged(ActiveTimelineChanged message)
        {
            SetTimeLine(message.Timeline);
        }
        /// <summary>
        /// Message callback for the currently selected node, (For example if scoping is 
        /// enabled we need to filter out keyframes local to only this node)
        /// </summary>
        /// <param name="message"></param>
        private void ActiveNodeChanged(SelectedNodeChanged message)
        {
            ResetTimeline(TimelineStart,TimelineEnd);
        }
        /// <summary>
        /// Set the range for the curent timeline
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void ResetTimeline(int start,int end)
        {
            if(timeline==null)
            {
                return;
            }

            var keyframes = timeline.GetKeyframeBetweenOnNode(activeNode, start, end);

            foreach(var frameSet in keyframes)
            {

            }
        }
        /// <summary>
        /// Sets the current timeline instance and binds/unbinds events to it
        /// </summary>
        /// <param name="newTimeline"></param>
        private void SetTimeLine(KeyframeTimeline newTimeline)
        {
            if(timeline != null)
            {
                timeline.KeyframeAdded -= NewTimeline_KeyframeAdded;
            }
            newTimeline.KeyframeAdded += NewTimeline_KeyframeAdded;
            timeline = newTimeline;
        }
        /// <summary>
        /// Event for when a keyframe is added to the timeline
        /// </summary>
        /// <param name="key"></param>
        private void NewTimeline_KeyframeAdded(Keyframe key)
        {
            throw new NotImplementedException();
        }
    }
}
