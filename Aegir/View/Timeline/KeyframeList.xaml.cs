using Aegir.ViewModel.Timeline;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aegir.View.Timeline
{

    internal class KeyframeListItem : ObservableObject
    {
        public int Time { get; private set; }
        public KeyframeViewModel KeyVM { get; private set; }

        private double xpos;

        public double TimelinePositionX
        {
            get { return xpos; }
            set
            {
                xpos = value;
                RaisePropertyChanged();
            }
        }

        public KeyframeListItem(KeyframeViewModel viewModel)
        {
            this.KeyVM = viewModel;
            this.Time = viewModel.Time;
        }
    }
    /// <summary>
    /// Interaction logic for KeyframeList.xaml
    /// </summary>
    public partial class KeyframeList : UserControl
    {

        private ObservableCollection<KeyframeListItem> keyItems;
        /// <summary>
        /// The total distance dragged in the current drag operation
        /// </summary>
        private double currentDeltaStepDistance;

        private int currentOperationFramesDragged;
        /// <summary>
        /// Set to 'true' when the left mouse-button is down.
        /// </summary>
        private bool isLeftMouseButtonDownOnCanvas = false;

        /// <summary>
        /// Set to 'true' when dragging the 'selection rectangle'.
        /// Dragging of the selection rectangle only starts when the left mouse-button is held down and the mouse-cursor
        /// is moved more than a threshold distance.
        /// </summary>
        private bool isDraggingSelectionRect = false;

        /// <summary>
        /// Records the location of the mouse (relative to the keyframe ruler) when the left-mouse button has pressed down.
        /// </summary>
        private Point origMouseDownPoint;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before drag-selection begins.
        /// </summary>
        private static readonly double DragThreshold = 5;

        /// <summary>
        /// Set to 'true' when the left mouse-button is held down on a keyframe.
        /// </summary>
        private bool isLeftMouseDownOnKeyframe = false;

        /// <summary>
        /// Set to 'true' when the left mouse-button and control are held down on a keyframe.
        /// </summary>
        private bool isLeftMouseAndControlDownOnKeyframe = false;

        /// <summary>
        /// Set to 'true' when dragging a keyframe.
        /// </summary>
        private bool isDraggingKeyframe = false;



        public ObservableCollection<KeyframeViewModel> Keyframes
        {
            get { return (ObservableCollection<KeyframeViewModel>)GetValue(KeyframesProperty); }
            set { SetValue(KeyframesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Keyframes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyframesProperty =
            DependencyProperty.Register(nameof(Keyframes), 
                typeof(ObservableCollection<KeyframeViewModel>), 
                typeof(KeyframeList), 
                new PropertyMetadata(KeyframeCollectionChanged));

        public int TimeRangeStart
        {
            get { return (int)GetValue(TimerangeStartProperty); }
            set
            {
                SetValue(TimerangeStartProperty, value);
                TimeRangeChanged();
            }
        }

        // Using a DependencyProperty as the backing store for TimerangeStart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimerangeStartProperty =
            DependencyProperty.Register(nameof(TimeRangeStart), typeof(int), typeof(KeyframeList), new PropertyMetadata(0));

        public int TimeRangeEnd
        {
            get { return (int)GetValue(TimerangeEndProperty); }
            set
            {
                SetValue(TimerangeEndProperty, value);
                TimeRangeChanged();
            }
        }

        // Using a DependencyProperty as the backing store for TimerangeEnd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimerangeEndProperty =
            DependencyProperty.Register(nameof(TimeRangeEnd), typeof(int), typeof(KeyframeList), new PropertyMetadata(0));


        public KeyframeList()
        {
            InitializeComponent();
            keyItems = new ObservableCollection<KeyframeListItem>();
            listBox.ItemsSource = keyItems;
        }
        private static void KeyframeCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KeyframeList view = d as KeyframeList;
            if (view != null)
            {
                view.KeyframeCollectionUpdated();
            }
        }


        private void TimeRangeChanged()
        {
            InvalidatePositions();
        }
        private void KeyframeCollectionUpdated()
        {
            Keyframes.CollectionChanged += KeyframeCollectionChanged;
        }

        private void KeyframeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach(KeyframeViewModel key in e.NewItems) { AddKeyframes(key); }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach(KeyframeViewModel key in e.OldItems) { RemoveKeyframes(key); }
                    break;
                default:
                    break;
            }
        }
        private void AddKeyframes(KeyframeViewModel keyVM)
        {
            KeyframeListItem key = new KeyframeListItem(keyVM);
            double position = GetCanvasPosition(keyVM.Time);
            key.TimelinePositionX = position;

            keyItems.Add(key);
        }
        private void RemoveKeyframes(KeyframeViewModel keyVM)
        {

        }

        private void InvalidatePositions()
        {

        }
        private double GetCanvasPosition(int time)
        {
            double stepSize = (ActualWidth - 20) / (TimeRangeEnd - TimeRangeStart);
            double leftOffset = stepSize * time + 2;
            return leftOffset;
        }
        /// <summary>
        /// Event raised when the mouse is pressed down on a keyframe.
        /// </summary>
        private void Keyframe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            FrameworkElement keyframe = (FrameworkElement)sender;
            KeyframeListItem keyframeViewmodel = (KeyframeListItem)keyframe.DataContext;

            isLeftMouseDownOnKeyframe = true;

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                //
                // Control key was held down.
                // This means that the keyframe is being added to or removed 
                // from the existing selection. Don't do anything yet, 
                // we will act on this later in the MouseUp event handler.
                //
                isLeftMouseAndControlDownOnKeyframe = true;
            }
            else
            {
                //
                // Control key is not held down.
                //
                isLeftMouseAndControlDownOnKeyframe = false;

                if (this.listBox.SelectedItems.Count == 0)
                {
                    //
                    // Nothing already selected, select the item.
                    //
                    this.listBox.SelectedItems.Add(keyframeViewmodel);
                }
                else if (this.listBox.SelectedItems.Contains(keyframeViewmodel))
                {
                    // 
                    // Item is already selected, do nothing.
                    // We will act on this in the MouseUp if there was no drag operation.
                    //
                }
                else
                {
                    //
                    // Item is not selected.
                    // Deselect all, and select the item.
                    //
                    this.listBox.SelectedItems.Clear();
                    this.listBox.SelectedItems.Add(keyframeViewmodel);
                }
            }

            keyframe.CaptureMouse();
            origMouseDownPoint = e.GetPosition(GridContainer);
            listBox.Focus();
            Keyboard.Focus(listBox);
            e.Handled = true;
        }

        /// <summary>
        /// Event raised when the mouse is released on a keyframe.
        /// </summary>
        private void Keyframe_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isLeftMouseDownOnKeyframe)
            {
                FrameworkElement keyframe = (FrameworkElement)sender;
                KeyframeListItem keyframeViewModel = (KeyframeListItem)keyframe.DataContext;

                if (!isDraggingKeyframe)
                {
                    //
                    // Execute mouse up selection logic only if there was no drag operation.
                    //
                    if (isLeftMouseAndControlDownOnKeyframe)
                    {
                        //
                        // Control key was held down.
                        // Toggle the selection.
                        //
                        if (this.listBox.SelectedItems.Contains(keyframeViewModel))
                        {
                            //
                            // Item was already selected, control-click removes it from the selection.
                            //
                            this.listBox.SelectedItems.Remove(keyframeViewModel);
                        }
                        else
                        {
                            // 
                            // Item was not already selected, control-click adds it to the selection.
                            //
                            this.listBox.SelectedItems.Add(keyframeViewModel);
                        }
                    }
                    else
                    {
                        //
                        // Control key was not held down.
                        //
                        if (this.listBox.SelectedItems.Count == 1 &&
                            this.listBox.SelectedItem == keyframeViewModel)
                        {
                            //
                            // The item that was clicked is already the only selected item.
                            // Don't need to do anything.
                            //
                        }
                        else
                        {
                            //
                            // Clear the selection and select the clicked item as the only selected item.
                            //
                            this.listBox.SelectedItems.Clear();
                            this.listBox.SelectedItems.Add(keyframeViewModel);
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("Frames Moved " + currentOperationFramesDragged);
                    bool hasKeyConflict = false;

                    foreach(KeyframeListItem key in this.listBox.SelectedItems)
                    {
                        int newTime = key.Time + currentOperationFramesDragged;
                        if(Keyframes.Any(x=>x.Time == newTime))
                        {
                            hasKeyConflict = true;
                        }
                    }
                    if(hasKeyConflict)
                    {
                        MessageBoxResult confirmBox = MessageBox.Show("One or more of the dragged keyframes will overwrite an existing keyframe", "Overwrite keyframes", MessageBoxButton.OKCancel);

                        if (confirmBox == MessageBoxResult.Yes)
                        {
                            Debug.WriteLine("Overwriting");
                        }
                        else
                        {
                            Debug.WriteLine("Dont overwrite");
                            double stepSize = (ActualWidth - 20) / (TimeRangeEnd - TimeRangeStart);
                            foreach (KeyframeListItem key in this.listBox.SelectedItems)
                            {
                                key.TimelinePositionX = stepSize * currentOperationFramesDragged;
                            }
                        }
                    }
                }

                keyframe.ReleaseMouseCapture();
                isLeftMouseDownOnKeyframe = false;
                isLeftMouseAndControlDownOnKeyframe = false;

                e.Handled = true;
            }

            isDraggingKeyframe = false;
            currentDeltaStepDistance = 0;
            currentOperationFramesDragged = 0;
        }

        /// <summary>
        /// Event raised when the mouse is moved over a keyframe.
        /// </summary>
        private void Keyframe_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingKeyframe)
            {
                //
                // Drag-move selected keyframes.
                //
                Point curMouseDownPoint = e.GetPosition(GridContainer);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;

                origMouseDownPoint = curMouseDownPoint;
                currentDeltaStepDistance += dragDelta.X;

                double stepSize = (ActualWidth - 20) / (TimeRangeEnd - TimeRangeStart);

                if(Math.Abs(currentDeltaStepDistance)>=stepSize)
                {
                    foreach (KeyframeListItem keyframe in this.listBox.SelectedItems)
                    {
                        keyframe.TimelinePositionX += currentDeltaStepDistance;
                        
                        //keyframe.CanvasY += dragDelta.Y;
                    }
                    if(currentDeltaStepDistance>0)
                    {
                        currentOperationFramesDragged++;
                    }
                    else
                    {
                        currentOperationFramesDragged--;
                    }
                    currentDeltaStepDistance = 0;
                }

            }
            else if (isLeftMouseDownOnKeyframe)
            {
                //
                // The user is left-dragging the keyframe,
                // but don't initiate the drag operation until
                // the mouse cursor has moved more than the threshold value.
                //
                Point curMouseDownPoint = e.GetPosition(GridContainer);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance > DragThreshold)
                {
                    //
                    // When the mouse has been dragged more than the threshold value commence dragging the keyframe.
                    //
                    isDraggingKeyframe = true;
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised when the user presses down the left mouse-button.
        /// </summary>
        private void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //
                //  Clear selection immediately when starting drag selection.
                //
                listBox.SelectedItems.Clear();

                isLeftMouseButtonDownOnCanvas = true;
                origMouseDownPoint = e.GetPosition(GridContainer);

                GridContainer.CaptureMouse();
                listBox.Focus();
                Keyboard.Focus(listBox);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised when the user releases the left mouse-button.
        /// </summary>
        private void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                bool wasDragSelectionApplied = false;

                if (isDraggingSelectionRect)
                {
                    //
                    // Drag selection has ended, apply the 'selection rectangle'.
                    //

                    isDraggingSelectionRect = false;
                    ApplyDragSelectionRect();

                    e.Handled = true;
                    wasDragSelectionApplied = true;
                }

                if (isLeftMouseButtonDownOnCanvas)
                {
                    isLeftMouseButtonDownOnCanvas = false;
                    GridContainer.ReleaseMouseCapture();

                    e.Handled = true;
                }

                if (!wasDragSelectionApplied)
                {
                    //
                    // A click and release in empty space clears the selection.
                    //
                    listBox.SelectedItems.Clear();
                }
            }
        }

        /// <summary>
        /// Event raised when the user moves the mouse button.
        /// </summary>
        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingSelectionRect)
            {
                //
                // Drag selection is in progress.
                //
                Point curMouseDownPoint = e.GetPosition(GridContainer);
                UpdateDragSelectionRect(origMouseDownPoint, curMouseDownPoint);

                e.Handled = true;
            }
            else if (isLeftMouseButtonDownOnCanvas)
            {
                //
                // The user is left-dragging the mouse,
                // but don't initiate drag selection until
                // they have dragged past the threshold value.
                //
                Point curMouseDownPoint = e.GetPosition(GridContainer);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance > DragThreshold)
                {
                    //
                    // When the mouse has been dragged more than the threshold value commence drag selection.
                    //
                    isDraggingSelectionRect = true;
                    InitDragSelectionRect(origMouseDownPoint, curMouseDownPoint);
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Initialize the rectangle used for drag selection.
        /// </summary>
        private void InitDragSelectionRect(Point pt1, Point pt2)
        {
            UpdateDragSelectionRect(pt1, pt2);

            dragSelectionCanvas.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Update the position and size of the rectangle used for drag selection.
        /// </summary>
        private void UpdateDragSelectionRect(Point pt1, Point pt2)
        {
            double x, y, width, height;

            //
            // Determine x,y,width and height of the rect inverting the points if necessary.
            // 

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            //
            // Update the coordinates of the rectangle used for drag selection.
            //
            Canvas.SetLeft(dragSelectionBorder, x);
            Canvas.SetTop(dragSelectionBorder, 2);
            dragSelectionBorder.Width = width;
            dragSelectionBorder.Height = 20;
        }

        /// <summary>
        /// Select all elements that are in the drag selection rectangle.
        /// </summary>
        private void ApplyDragSelectionRect()
        {
            dragSelectionCanvas.Visibility = Visibility.Collapsed;

            double x = Canvas.GetLeft(dragSelectionBorder);
            double y = Canvas.GetTop(dragSelectionBorder);
            double width = dragSelectionBorder.Width;
            double height = dragSelectionBorder.Height;
            Rect dragRect = new Rect(x, y, width, height);

            //
            // Inflate the drag selection-rectangle by 1/10 of its size to 
            // make sure the intended item is selected.
            //
            dragRect.Inflate(width / 10, height / 10);

            //
            // Clear the current selection.
            //
            listBox.SelectedItems.Clear();

            //
            // Find and select all the list box items.
            //
            foreach (KeyframeListItem key in keyItems)
            {
                Rect itemRect = new Rect(key.TimelinePositionX, 2, 8, 20);
                if (dragRect.Contains(itemRect))
                {
                    listBox.SelectedItems.Add(key);
                }
            }
        }

        private void KeyframeListControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InvalidatePositions();
        }
    }
}
