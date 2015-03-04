using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aegir.Map
{
    /// <summary>Displays a map using data from openstreetmap.org.</summary>
    public sealed partial class MapCanvas : Canvas
    {
        /// <summary>Identifies the Latitude attached property.</summary>
        public static readonly DependencyProperty LatitudeProperty =
            DependencyProperty.RegisterAttached("Latitude", typeof(double), typeof(MapCanvas), new PropertyMetadata(double.PositiveInfinity, OnLatitudeLongitudePropertyChanged));

        /// <summary>Identifies the Longitude attached property.</summary>
        public static readonly DependencyProperty LongitudeProperty =
            DependencyProperty.RegisterAttached("Longitude", typeof(double), typeof(MapCanvas), new PropertyMetadata(double.PositiveInfinity, OnLatitudeLongitudePropertyChanged));

        /// <summary>Identifies the Viewport dependency property.</summary>
        public static readonly DependencyProperty ViewportProperty;

        /// <summary>Identifies the Zoom dependency property.</summary>
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(int), typeof(MapCanvas), new UIPropertyMetadata(0, OnZoomPropertyChanged, OnZoomPropertyCoerceValue));

        private static readonly DependencyPropertyKey ViewportKey =
            DependencyProperty.RegisterReadOnly("Viewport", typeof(Rect), typeof(MapCanvas), new PropertyMetadata());

        private TilePanel _tilePanel = new TilePanel();
        private Image _cache = new Image();
        private int _updateCount;
        private bool _mouseCaptured;
        private Point _previousMouse;
        private MapOffset _offsetX;
        private MapOffset _offsetY;
        private TranslateTransform _translate = new TranslateTransform();

        /// <summary>Initializes static members of the MapCanvas class. Also registers command bindings.</summary>
        static MapCanvas()
        {
            ViewportProperty = ViewportKey.DependencyProperty; // Need to set it here after ViewportKey has been initialized.

            CommandManager.RegisterClassCommandBinding(
                typeof(MapCanvas), new CommandBinding(ComponentCommands.MoveDown, (sender, e) => Pan(sender, e.Command, 0, -1)));
            CommandManager.RegisterClassCommandBinding(
                typeof(MapCanvas), new CommandBinding(ComponentCommands.MoveLeft, (sender, e) => Pan(sender, e.Command, 1, 0)));
            CommandManager.RegisterClassCommandBinding(
                typeof(MapCanvas), new CommandBinding(ComponentCommands.MoveRight, (sender, e) => Pan(sender, e.Command, -1, 0)));
            CommandManager.RegisterClassCommandBinding(
                typeof(MapCanvas), new CommandBinding(ComponentCommands.MoveUp, (sender, e) => Pan(sender, e.Command, 0, 1)));

            CommandManager.RegisterClassCommandBinding(
                typeof(MapCanvas), new CommandBinding(NavigationCommands.DecreaseZoom, (sender, e) => ((MapCanvas)sender).Zoom--));
            CommandManager.RegisterClassCommandBinding(
                typeof(MapCanvas), new CommandBinding(NavigationCommands.IncreaseZoom, (sender, e) => ((MapCanvas)sender).Zoom++));
        }

        /// <summary>Initializes a new instance of the MapCanvas class.</summary>
        public MapCanvas()
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
#endif

            _offsetX = new MapOffset(_translate.GetType().GetProperty("X"), this.OnOffsetChanged);
            _offsetY = new MapOffset(_translate.GetType().GetProperty("Y"), this.OnOffsetChanged);

            _tilePanel.RenderTransform = _translate;
            this.Background = Brushes.Transparent; // Register all mouse clicks
            this.Children.Add(_cache);
            this.Children.Add(_tilePanel);
            this.ClipToBounds = true;
            this.Focusable = true;
            this.FocusVisualStyle = null;
            this.SnapsToDevicePixels = true;

            //We need to way for this control to be loaded before we can center our map
            this.Loaded += MapCanvas_Loaded;
            
        }

        void MapCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Center(59.91872, 10.68599, 15);
        }

        /// <summary>Gets the visible area of the map in latitude/longitude coordinates.</summary>
        public Rect Viewport
        {
            get { return (Rect)this.GetValue(ViewportProperty); }
            private set { this.SetValue(ViewportKey, value); }
        }

        /// <summary>Gets or sets the zoom level of the map.</summary>
        public int Zoom
        {
            get { return (int)this.GetValue(ZoomProperty); }
            set { this.SetValue(ZoomProperty, value); }
        }

        /// <summary>Gets the value of the Latitude attached property for a given depencency object.</summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The Latitude coordinate of the specified element.</returns>
        public static double GetLatitude(DependencyObject obj)
        {
            return (double)obj.GetValue(LatitudeProperty);
        }

        /// <summary>Gets the value of the Longitude attached property for a given depencency object.</summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The Longitude coordinate of the specified element.</returns>
        public static double GetLongitude(DependencyObject obj)
        {
            return (double)obj.GetValue(LongitudeProperty);
        }

        /// <summary>Sets the value of the Latitude attached property for a given depencency object.</summary>
        /// <param name="obj">The element to which the property value is written.</param>
        /// <param name="value">Sets the Latitude coordinate of the specified element.</param>
        public static void SetLatitude(DependencyObject obj, double value)
        {
            obj.SetValue(LatitudeProperty, value);
        }

        /// <summary>Sets the value of the Longitude attached property for a given depencency object.</summary>
        /// <param name="obj">The element to which the property value is written.</param>
        /// <param name="value">Sets the Longitude coordinate of the specified element.</param>
        public static void SetLongitude(DependencyObject obj, double value)
        {
            obj.SetValue(LongitudeProperty, value);
        }

        /// <summary>Centers the map on the specified coordinates.</summary>
        /// <param name="latitude">The latitude cooridinate.</param>
        /// <param name="longitude">The longitude coordinates.</param>
        /// <param name="zoom">The zoom level for the map.</param>
        public void Center(double latitude, double longitude, int zoom)
        {
            this.BeginUpdate();
            this.Zoom = zoom;
            _offsetX.CenterOn(TileGenerator.GetTileX(longitude, this.Zoom));
            _offsetY.CenterOn(TileGenerator.GetTileY(latitude, this.Zoom));
            this.EndUpdate();
        }

        /// <summary>Centers the map on the specified coordinates, calculating the required zoom level.</summary>
        /// <param name="latitude">The latitude cooridinate.</param>
        /// <param name="longitude">The longitude coordinates.</param>
        /// <param name="size">The minimum size that must be visible, centered on the coordinates.</param>
        public void Center(double latitude, double longitude, Size size)
        {
            double left = TileGenerator.GetTileX(longitude - (size.Width / 2.0), 0);
            double right = TileGenerator.GetTileX(longitude + (size.Width / 2.0), 0);
            double top = TileGenerator.GetTileY(latitude - (size.Height / 2.0), 0);
            double bottom = TileGenerator.GetTileY(latitude + (size.Height / 2.0), 0);

            double height = (top - bottom) * TileGenerator.TileSize;
            double width = (right - left) * TileGenerator.TileSize;
            int zoom = Math.Min(TileGenerator.GetZoom(this.ActualHeight / height), TileGenerator.GetZoom(this.ActualWidth / width));
            this.Center(latitude, longitude, zoom);
        }

        /// <summary>Creates a static image of the current view.</summary>
        /// <returns>An image of the current map.</returns>
        public ImageSource CreateImage()
        {

            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Default);
            bitmap.Render(_tilePanel);
            bitmap.Freeze();
            return bitmap;
        }

        /// <summary>Calculates the coordinates of the specifed point.</summary>
        /// <param name="point">A point, in pixels, relative to the top left corner of the control.</param>
        /// <returns>A Point filled with the Latitude (Y) and Longitude (X) of the specifide point.</returns>
        public Point GetLocation(Point point)
        {
            Point output = new Point();
            output.X = TileGenerator.GetLongitude((_offsetX.Pixels + point.X) / TileGenerator.TileSize, this.Zoom);
            output.Y = TileGenerator.GetLatitude((_offsetY.Pixels + point.Y) / TileGenerator.TileSize, this.Zoom);
            return output;
        }

        /// <summary>Tries to capture the mouse to enable dragging of the map.</summary>
        /// <param name="e">The MouseButtonEventArgs that contains the event data.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.Focus(); // Make sure we get the keyboard
            if (this.CaptureMouse())
            {
                _mouseCaptured = true;
                _previousMouse = e.GetPosition(null);
            }
        }

        /// <summary>Releases the mouse capture and stops dragging of the map.</summary>
        /// <param name="e">The MouseButtonEventArgs that contains the event data.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            this.ReleaseMouseCapture();
            _mouseCaptured = false;
        }

        /// <summary>Drags the map, if the mouse was succesfully captured.</summary>
        /// <param name="e">The MouseEventArgs that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_mouseCaptured)
            {
                this.BeginUpdate();
                Point position = e.GetPosition(null);
                _offsetX.Translate(position.X - _previousMouse.X);
                _offsetY.Translate(position.Y - _previousMouse.Y);
                _previousMouse = position;
                this.EndUpdate();
            }
        }

        /// <summary>Alters the zoom of the map, maintaing the same point underneath the mouse at the new zoom level.</summary>
        /// <param name="e">The MouseWheelEventArgs that contains the event data.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            int newZoom = TileGenerator.GetValidZoom(this.Zoom + (e.Delta / Mouse.MouseWheelDeltaForOneLine));
            Point mouse = e.GetPosition(this);

            this.BeginUpdate();
            _offsetX.ChangeZoom(newZoom, mouse.X);
            _offsetY.ChangeZoom(newZoom, mouse.Y);
            this.Zoom = newZoom; // Set this after we've altered the offsets
            this.EndUpdate();
        }

        /// <summary>Notifies child controls that the size has changed.</summary>
        /// <param name="sizeInfo">
        /// The packaged parameters (SizeChangedInfo), which includes old and new sizes, and which dimension actually changes.
        /// </param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {

            base.OnRenderSizeChanged(sizeInfo);
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
#endif
            this.BeginUpdate();
            _offsetX.ChangeSize(sizeInfo.NewSize.Width);
            _offsetY.ChangeSize(sizeInfo.NewSize.Height);
            _tilePanel.Width = sizeInfo.NewSize.Width;
            _tilePanel.Height = sizeInfo.NewSize.Height;
            this.EndUpdate();
        }

        private static bool IsKeyboardCommand(RoutedCommand command)
        {
            foreach (var gesture in command.InputGestures)
            {
                var key = gesture as KeyGesture;
                if (key != null)
                {
                    if (Keyboard.IsKeyDown(key.Key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void OnLatitudeLongitudePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Search for a MapControl parent
            MapCanvas canvas = null;
            FrameworkElement child = d as FrameworkElement;
            while (child != null)
            {
                canvas = child as MapCanvas;
                if (canvas != null)
                {
                    break;
                }
                child = child.Parent as FrameworkElement;
            }
            if (canvas != null)
            {
                canvas.RepositionChildren();
            }
        }

        private static void OnZoomPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapCanvas)d).OnZoomChanged();
        }

        private static object OnZoomPropertyCoerceValue(DependencyObject d, object baseValue)
        {
            return TileGenerator.GetValidZoom((int)baseValue);
        }

        private static void Pan(object sender, ICommand command, double x, double y)
        {
            if (!IsKeyboardCommand((RoutedCommand)command)) // Move a whole square instead of a pixel if it wasn't the keyboard who sent it
            {
                x *= TileGenerator.TileSize;
                y *= TileGenerator.TileSize;
            }
            MapCanvas instance = (MapCanvas)sender;
            instance._offsetX.AnimateTranslate(x);
            instance._offsetY.AnimateTranslate(y);
            instance.Focus();
        }

        private void OnOffsetChanged(object sender, EventArgs e)
        {
            this.BeginUpdate();
            MapOffset offset = (MapOffset)sender;
            offset.Property.SetValue(_translate, offset.Offset, null);
            this.EndUpdate();
        }

        private void OnZoomChanged()
        {
            this.BeginUpdate();
            _offsetX.ChangeZoom(this.Zoom, this.ActualWidth / 2.0);
            _offsetY.ChangeZoom(this.Zoom, this.ActualHeight / 2.0);
            _tilePanel.Zoom = this.Zoom;
            this.EndUpdate();
        }

        private void BeginUpdate()
        {
            _updateCount++;
        }

        private void EndUpdate()
        {
            System.Diagnostics.Debug.Assert(_updateCount != 0, "Must call BeginUpdate first");
            if (--_updateCount == 0)
            {
                _tilePanel.LeftTile = _offsetX.Tile;
                _tilePanel.TopTile = _offsetY.Tile;
                if (_tilePanel.RequiresUpdate)
                {
                    _cache.Visibility = Visibility.Visible; // Display a pretty picture while we play with the tiles
                    _tilePanel.Update(); // This will block our thread for a while (UI events will still be processed)
                    this.RepositionChildren();
                    _cache.Visibility = Visibility.Hidden;
                    _cache.Source = this.CreateImage(); // Save our image for later
                }

                // Update viewport
                Point topleft = this.GetLocation(new Point(0, 0));
                Point bottomRight = this.GetLocation(new Point(this.ActualWidth, this.ActualHeight));
                this.Viewport = new Rect(topleft, bottomRight);
            }
        }

        private void RepositionChildren()
        {
            foreach (UIElement element in this.Children)
            {
                double latitude = GetLatitude(element);
                double longitude = GetLongitude(element);
                if (latitude != double.PositiveInfinity && longitude != double.PositiveInfinity)
                {
                    double x = (TileGenerator.GetTileX(longitude, this.Zoom) - _offsetX.Tile) * TileGenerator.TileSize;
                    double y = (TileGenerator.GetTileY(latitude, this.Zoom) - _offsetY.Tile) * TileGenerator.TileSize;
                    Canvas.SetLeft(element, x);
                    Canvas.SetTop(element, y);
                    element.RenderTransform = _translate;
                }
            }
        }
    }
}