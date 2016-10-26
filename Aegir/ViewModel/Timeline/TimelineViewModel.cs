using Aegir.Messages.ObjectTree;
using Aegir.Messages.Selection;
using Aegir.Messages.Timeline;
using Aegir.Mvvm;
using Aegir.View.Timeline;
using AegirCore.Keyframe;
using AegirCore.Messages;
using AegirCore.Scene;
using GalaSoft.MvvmLight.Command;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TinyMessenger;

namespace Aegir.ViewModel.Timeline
{
    /// <summary>
    /// Viewmodel for timelineviewmodel
    /// </summary>
    public class TimelineViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TimelineViewModel));

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
            set
            {
                currentTimelinePosition = value;
                //log.DebugFormat("Time Updated to {0}", value);
                UpdateTime();
                RaisePropertyChanged();
            }
        }

        public PlaybackMode PlayMode
        {
            get
            {
                if (Engine != null)
                {
                    return Engine.PlaybackMode;
                }
                else
                {
                    return PlaybackMode.PAUSED;
                }
            }
            set
            {
                if (Engine != null)
                {
                    log.DebugFormat("Setting keyframe engine playmode to {0}", value);
                    Engine.PlaybackMode = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Where our timeline starts
        /// </summary>
        public int TimelineStart
        {
            get { return timelineStart; }
            set
            {
                //log.Debug("TimelineStart: " + value);
                timelineStart = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand playPauseCommand;
        private DateTime lastNotifyProxyProperty;
        private KeyframeListItem selectedKey;

        public RelayCommand PlayPauseCommand
        {
            get { return playPauseCommand; }
            set { playPauseCommand = value; }
        }

        internal void RemoveKey(Keyframe key)
        {
            Engine.Keyframes.RemoveKey(key);
        }

        /// <summary>
        /// Where our timeline ends
        /// </summary>
        public int TimelineEnd
        {
            get { return timelineEnd; }
            set
            {
                //log.Debug("TimelineEnd: " + value);
                timelineEnd = value;
                RaisePropertyChanged();
            }
        }

        public bool LoopPlayback
        {
            get
            {
                return Engine.LoopPlayback;
            }
            set
            {
                Engine.LoopPlayback = value;
            }
        }

        public bool Reverse
        {
            get { return Engine.ReverseOnEnd; }
            set { Engine.ReverseOnEnd = value; }
        }

        public int PlaybackStart
        {
            get { return Engine.PlaybackStart; }
            set { Engine.PlaybackEnd = value; }
        }

        public int PlaybackEnd
        {
            get { return Engine.PlaybackEnd; }
            set { Engine.PlaybackEnd = value; }
        }

        public KeyframeListItem SelectedKey
        {
            get { return selectedKey; }
            set
            {
                if (selectedKey != value)
                {
                    selectedKey = value;
                    UpdateSelectedKey(value);
                }
            }
        }

        public KeyframeEngine Engine { get; private set; }
        public double NotifyPropertyUpdateRate { get; private set; }

        /// <summary>
        /// Instantiates a new timeline viewmodel
        /// </summary>
        public TimelineViewModel(ITinyMessengerHub messenger, KeyframeEngine engine)
        {
            NotifyPropertyUpdateRate = 50;
            TimelineStart = 0;
            TimelineEnd = 100;
            Messenger = messenger;
            //Set up engine
            Engine = engine;
            Engine.CurrentTimeChanged += Engine_CurrentTimeChanged;
            SetTimeLine(Engine.Keyframes);
            //MessengerInstance.Register<SelectedNodeChanged>(this, ActiveNodeChanged);
            //MessengerInstance.Register<ActiveTimelineChanged>(this, TimelineChanged);
            Messenger.Subscribe<SelectedNodeChanged>(ActiveNodeChanged);
            Messenger.Subscribe<InvalidateEntity>(OnInvalidateEntitiesMessage);
            AddKeyframeCommand = new RelayCommand(CaptureKeyframes, CanCaptureKeyframes);
            Keyframes = new ObservableCollection<KeyframeViewModel>();
            playPauseCommand = new RelayCommand(TogglePlayPauseState);
        }

        private void UpdateSelectedKey(KeyframeListItem value)
        {
            //RaisePropertyChanged();
            Debug.WriteLine($"Selected Key Changed {SelectedKey?.Time}");
            Messenger.Publish<SelectionChanged>(new SelectionChanged(this, value));
        }

        private void OnInvalidateEntitiesMessage(InvalidateEntity node)
        {
            DateTime now = DateTime.Now;
            double timeDifference = (now - lastNotifyProxyProperty).TotalMilliseconds;
            if (timeDifference > NotifyPropertyUpdateRate || true)
            {
                currentTimelinePosition = Engine.Time;
                RaisePropertyChanged("Time");
                lastNotifyProxyProperty = now;
            }
        }

        private void TogglePlayPauseState()
        {
            if (PlayMode == PlaybackMode.PLAYING)
            {
                PlayMode = PlaybackMode.PAUSED;
            }
            else if (PlayMode == PlaybackMode.PAUSED)
            {
                PlayMode = PlaybackMode.PLAYING;
            }
        }

        private void UpdateTime()
        {
            Engine.Time = Time;
        }

        /// <summary>
        /// Requests the timeline to create a new "Snapshot" of keyframes for the given timeline time
        /// </summary>
        private void CaptureKeyframes()
        {
            log.DebugFormat("SetKeyframe at frame {0} on {1}",
                            Time, activeNode?.Name);
            Stopwatch sw = Stopwatch.StartNew();
            Engine.CaptureAndAddToTimeline(activeNode, Time);
            sw.Stop();
            log.DebugFormat("SetKeyframe finished, used {0} ms",
                            sw.Elapsed.TotalMilliseconds);
        }

        private bool CanCaptureKeyframes()
        {
            return activeNode != null && Engine.CanCaptureNode(activeNode);
        }

        /// <summary>
        /// Message callback for timeline instance changed
        /// </summary>
        /// <param name="message">A message containing the new timeline</param>
        private void TimelineChanged(ActiveTimelineChanged message)
        {
            log.DebugFormat("ActiveTimelineChanged Received, Timeline changed to {0}",
                            message?.Timeline.ToString());
            SetTimeLine(message.Timeline);
            Engine = message.Engine;
        }

        private void Engine_CurrentTimeChanged(int newTime)
        {
            Time = newTime;
        }

        /// <summary>
        /// Message callback for the currently selected node, (For example if scoping is
        /// enabled we need to filter out keyframes local to only this node)
        /// </summary>
        /// <param name="message"></param>
        private void ActiveNodeChanged(SelectedNodeChanged message)
        {
            activeNode = message.Content.NodeSource;
            log.DebugFormat("SelectedNodeChanged Received, ActiveNode Changed to {0}",
                            activeNode?.Name);

            AddKeyframeCommand.RaiseCanExecuteChanged();
            //ResetTimeline(TimelineStart,TimelineEnd);
        }

        /// <summary>
        /// Set the range for the curent timeline
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void ResetTimeline(int start, int end)
        {
            if (timeline == null)
            {
                return;
            }

            //var keyframes = timeline.GetKeyframeBetweenOnNode(activeNode, start, end);

            //foreach(var frameSet in keyframes)
            //{
            //}
        }

        /// <summary>
        /// Sets the current timeline instance and binds/unbinds events to it
        /// </summary>
        /// <param name="newTimeline"></param>
        private void SetTimeLine(KeyframeTimeline newTimeline)
        {
            if (timeline != null)
            {
                timeline.KeyframeAdded -= Timeline_KeyframeAdded;
            }
            newTimeline.KeyframeAdded += Timeline_KeyframeAdded;
            timeline = newTimeline;
        }

        /// <summary>
        /// Event for when a keyframe is added to the timeline
        /// </summary>
        /// <param name="key"></param>

        private void Timeline_KeyframeAdded(Node node, int time, Keyframe key)
        {
            KeyframeViewModel keyVM = new KeyframeViewModel(key, time, this);
            Keyframes.Add(keyVM);
        }
    }
}