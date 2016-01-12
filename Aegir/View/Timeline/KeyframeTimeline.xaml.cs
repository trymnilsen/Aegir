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

        public List<Segment> segments;

        public KeyframeTimeline()
        {
            segments = new List<Segment>();
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
            //KeyFrameTimeLineRuler.Background = ticksBrush;
            //Set Ruler Thickness

            GenerateTickSegments(range, 5, 10);

        }
        private void GenerateTickSegments(int range, int numOfSegments, int numOfSegmentSteps)
        {
            double keyFrameTicksPadding = 10;
            KeyFrameTimeLineRuler.Children.Clear();
            segments.Clear();
            double stepSize = (ActualWidth-keyFrameTicksPadding*2) / range;
            while(!(stepSize<10 && stepSize>2))
            {
                if(stepSize>10)
                {
                    stepSize = stepSize / 2;
                }
                if(stepSize<2)
                {
                    stepSize = stepSize * 2;
                }
            }
            for (int i = 0; i < numOfSegments; i++)
            {
                double xOffset = i * stepSize * numOfSegmentSteps + keyFrameTicksPadding;
                Line[] tickLines = new Line[numOfSegmentSteps];
                //Small ticks
                for(int j=0; j < numOfSegmentSteps-1; j++)
                {
                    Line tick = new Line();
                    tick.X1 = xOffset + stepSize * j;
                    tick.X2 = xOffset + stepSize * j;
                    tick.Y1 = 0;
                    tick.Y2 = 10;
                    tick.Stroke = TicksColor;
                    tickLines[j] = tick;
                    this.KeyFrameTimeLineRuler.Children.Add(tick);
                }
                //Last tick of segment
                Line lastTick = new Line();
                lastTick.X1 = xOffset + stepSize * (numOfSegmentSteps-1);
                lastTick.X2 = xOffset + stepSize * (numOfSegmentSteps-1);
                lastTick.Y1 = 0;
                lastTick.Y2 = 20;
                lastTick.Stroke = TicksColor;
                tickLines[numOfSegmentSteps-1] = lastTick;

                this.KeyFrameTimeLineRuler.Children.Add(lastTick);

                segments.Add(new Segment(tickLines, 
                                        numOfSegmentSteps, 
                                        xOffset, 
                                        stepSize * numOfSegmentSteps));
            }
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
