using Aegir.Messages.ObjectTree;
using Aegir.Messages.Selection;
using Aegir.Messages.Timeline;
using Aegir.Mvvm;
using Aegir.Util;
using Aegir.View.Timeline;
using AegirLib.Keyframe;
using AegirLib.Keyframe.Timeline;
using AegirLib.Messages;
using AegirLib.Scene;
using GalaSoft.MvvmLight.Command;
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

        /// <summary>
        /// Backing store for if the timeline is scoped
        /// </summary>
        private bool isScoped;

        /// <summary>
        /// Backingstore for the timeline
        /// </summary>
        private KeyframeTimeline timeline;

        /// <summary>
        /// the currently used active entity
        /// </summary>
        private Entity activeEntity;

        private int timelineStart;
        private int timelineEnd;
        private int currentTimelinePosition;

        /// <summary>
        /// Command for creating keyframes at the current timeline
        /// value for the selected entity
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
        public bool IsScopedToEntity
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
                    Aegir.Util.DebugUtil.LogWithLocation($"Setting keyframe engine playmode to {value}");
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
        private KeyframeViewModel selectedKey;

        public RelayCommand PlayPauseCommand
        {
            get { return playPauseCommand; }
            set { playPauseCommand = value; }
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

        public KeyframeViewModel SelectedKey
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
            //SetTimeLine(Engine.Keyframes);
            //MessengerInstance.Register<SelectedNodeChanged>(this, ActiveNodeChanged);
            //MessengerInstance.Register<ActiveTimelineChanged>(this, TimelineChanged);
            Messenger.Subscribe<SelectedEntityChanged>(ActiveEntityChanged);
            Messenger.Subscribe<InvalidateEntity>(OnInvalidateEntitiesMessage);
            AddKeyframeCommand = new RelayCommand(CaptureKeyframes, CanCaptureKeyframes);
            Keyframes = new ObservableCollection<KeyframeViewModel>();
            playPauseCommand = new RelayCommand(TogglePlayPauseState);
        }

        private void UpdateSelectedKey(KeyframeViewModel value)
        {
            //RaisePropertyChanged();
            Debug.WriteLine($"Selected Key Changed {SelectedKey?.Time}");
            Messenger.Publish<SelectionChanged>(new SelectionChanged(this, value));
        }

        private void OnInvalidateEntitiesMessage(InvalidateEntity entity)
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
            Aegir.Util.DebugUtil.LogWithLocation($"SetKeyframe at frame {Time} on {activeEntity?.Name}");
            Stopwatch sw = Stopwatch.StartNew();
            Engine.Capture(activeEntity, Time);
            sw.Stop();
            Aegir.Util.DebugUtil.LogWithLocation($"SetKeyframe finished, used {sw.Elapsed.TotalMilliseconds} ms");
        }

        private bool CanCaptureKeyframes()
        {
            return activeEntity != null && Engine.CanCaptureEntity(activeEntity);
        }

        /// <summary>
        /// Message callback for timeline instance changed
        /// </summary>
        /// <param name="message">A message containing the new timeline</param>
        private void TimelineChanged(ActiveTimelineChanged message)
        {
            Aegir.Util.DebugUtil.LogWithLocation("ActiveTimelineChanged Received, Timeline changed to {message?.Timeline.ToString()}");
            SetTimeLine(message.Timeline);
            Engine = message.Engine;
        }

        private void Engine_CurrentTimeChanged(int newTime)
        {
            Time = newTime;
        }

        /// <summary>
        /// Message callback for the currently selected entity, (For example if scoping is
        /// enabled we need to filter out keyframes local to only this entity)
        /// </summary>
        /// <param name="message"></param>
        private void ActiveEntityChanged(SelectedEntityChanged message)
        {
            activeEntity = message.Content.EntitySource;
            Aegir.Util.DebugUtil.LogWithLocation($"SelectedEntityChanged Received, ActiveEntity Changed to {activeEntity?.Name}");
            AddKeyframeCommand.RaiseCanExecuteChanged();
            //Get the current Timeline for this entity
            KeyframeTimeline timeline = Engine.GetTimeline(activeEntity);
            SetTimeLine(timeline);
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
            if(!newTimeline.IsEmpty)
            {
                RebuildKeyframeViewModels();
            }
            newTimeline.KeyframeAdded += Timeline_KeyframeAdded;
            timeline = newTimeline;
        }

        private void RebuildKeyframeViewModels()
        {
            foreach(var key in timeline.Keys)
            {
                KeyframeViewModel keyVM = new KeyframeViewModel(key.Key);
                Keyframes.Add(keyVM);
            }
        }

        /// <summary>
        /// Event for when a keyframe is added to the timeline
        /// </summary>
        /// <param name="key"></param>

        private void Timeline_KeyframeAdded(KeyContainer key)
        {
            KeyframeViewModel keyVM = new KeyframeViewModel(key.Time);
            Keyframes.Add(keyVM);
        }
    }
}