using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for KeyframeTimeline.xaml
    /// </summary>
    public partial class KeyframeTimeline : UserControl
    {
        private TimelineRange visibleRange;
        private int viewPortScale;
        public int TimeRangeStart
        {
            get { return (int)GetValue(TimeRangeStartProperty); }
            set
            {
                SetValue(TimeRangeStartProperty, value);
            }
        }

        public static readonly DependencyProperty TimeRangeStartProperty =
            DependencyProperty.Register(nameof(TimeRangeStart),
                                typeof(int),
                                typeof(KeyframeTimeline),
                                new PropertyMetadata(
                                    0,
                                    new PropertyChangedCallback(TimeRangeChanged)
                                ));

        public int TimeRangeEnd
        {
            get { return (int)GetValue(TimeRangeEndProperty); }
            set
            {
                SetValue(TimeRangeEndProperty, value);
            }
        }

        public static readonly DependencyProperty TimeRangeEndProperty =
            DependencyProperty.Register(nameof(TimeRangeEnd),
                                typeof(int),
                                typeof(KeyframeTimeline),
                                new PropertyMetadata(
                                    100,
                                    new PropertyChangedCallback(TimeRangeChanged)
                                ));

        public Brush TicksColor
        {
            get { return (Brush)GetValue(TicksColorProperty); }
            set
            {
                SetValue(TicksColorProperty, value);
            }
        }

        public static readonly DependencyProperty TicksColorProperty =
            DependencyProperty.Register(nameof(TicksColor),
                                typeof(Brush),
                                typeof(KeyframeTimeline), 
                                new PropertyMetadata(
                                    new SolidColorBrush(Color.FromArgb(255,255,0,0)), 
                                    new PropertyChangedCallback(TicksColorChanged)
                                ));

        public KeyframeTimeline()
        {
            InitializeComponent();
        }
        public static void TicksColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KeyframeTimeline view = d as KeyframeTimeline;
            if (view != null)
            {
                view.InvalidateTimeline();
            }
        }
        public static void TimeRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("DP Callback, TimeRangeStartChanged");
            KeyframeTimeline view = d as KeyframeTimeline;
            if (view != null)
            {
                view.UpdateTimeRange();
            }
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InvalidateTimeline();
        }
        public void UpdateTimeRange()
        {
            Debug.WriteLine("TimeRange Update: " + TimeRangeStart + "/" + TimeRangeEnd);
            InvalidateTimeline();
        }
        private void InvalidateTimeline()
        {
            //Get range
            int range = TimeRangeEnd - TimeRangeStart;
            //For now always have 5 segments
            double segmentSize = (ActualWidth / range  ) * 10;
            
            //Align ticks/viewport with range and width
            Rect viewport = new Rect(0, 0, range, 20);
            //KeyFrameTimeLineRuler.Background = ticksBrush;
            //Set Ruler Thickness

        }

        private void GetTicksBrush(int stepsPerSegment, double segmentSize)
        {
            DrawingBrush brush = new DrawingBrush();
            //Set brush settings
            brush.Viewport = new Rect(0, 0, segmentSize, 20);
            brush.ViewportUnits = BrushMappingMode.Absolute;
            brush.Stretch = Stretch.None;
            brush.TileMode = TileMode.Tile;
            //Generate geometry

            GeometryGroup lines = new GeometryGroup();
            double stepSize = segmentSize / stepsPerSegment;
            //Add Lines
            for(int i = 1; i<stepsPerSegment; i++)
            {
                Point start = new Point(stepSize * i, 0);
                Point end = new Point(stepSize * i, 10);
                lines.Children.Add(new LineGeometry(start, end));
            }
            //Add last line of segment
            lines.Children.Add(new LineGeometry(new Point(segmentSize, 0), 
                                                new Point(segmentSize, 20)));

            //Set geometry as drawing
            GeometryDrawing drawing = new GeometryDrawing();
            drawing.Pen = new Pen(TicksColor, 1);
            drawing.Geometry = lines;

            brush.Drawing = drawing;

            //return brush;
        }
    }
}
