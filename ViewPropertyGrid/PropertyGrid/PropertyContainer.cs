using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ViewPropertyGrid.PropertyGrid
{
    public class PropertyContainer : Grid
    {

        private readonly SolidColorBrush bgColor = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush bgHighlightColor = new SolidColorBrush(Colors.LightBlue);

        private TextBlock textLabel;
        private FrameworkElement activeElement;
        private FrameworkElement inactiveElement;
        private Border keyWrapper;
        private Border valueWrapper;
        private string propertyName;
        public PropertyContainer(string propertyName, FrameworkElement valueControl)
            :this(propertyName,valueControl,null) { }

        public PropertyContainer(string propertyName, FrameworkElement valueControl, FrameworkElement inactiveElement)
        {
            this.propertyName = propertyName;
            this.activeElement = valueControl;
            this.inactiveElement = inactiveElement;

            //Add three columns
            ColumnDefinitions.Add(new ColumnDefinition() {  });
            ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(150),
                    SharedSizeGroup = "PropGridColumn"
                });

            //Create wrappers
            keyWrapper = new Border() { Name = "keywrapper", Background = new SolidColorBrush(Colors.White) };
            valueWrapper = new Border() { Name = "valuewrapper", Background = new SolidColorBrush(Colors.White) };
            
            
            Grid.SetColumn(keyWrapper, 0);
            Grid.SetColumn(valueWrapper, 1);

            this.Children.Add(keyWrapper);
            this.Children.Add(valueWrapper);

            textLabel = new TextBlock();
            textLabel.TextTrimming = TextTrimming.CharacterEllipsis;
            textLabel.ToolTip = propertyName;
            textLabel.Text = propertyName;

            keyWrapper.Child = textLabel;

            if(inactiveElement==null)
            {
                valueWrapper.Child = valueControl;
            }
            else
            {
                valueWrapper.Child = inactiveElement;
            }

            this.Focusable = true;

        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.keyWrapper.Background = bgHighlightColor;
            Debug.WriteLine("GotKeyboardFocus: " + propertyName);
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.keyWrapper.Background = bgHighlightColor;
            Debug.WriteLine("GotFocus: "+propertyName);
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            this.keyWrapper.Background = bgColor;
            Debug.WriteLine("Lost Focus " + propertyName);
        }
        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {

            //base.OnPreviewLostKeyboardFocus(e);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //DependencyObject focusScope = FocusManager.GetFocusScope(this);
            //FocusManager.SetFocusedElement(focusScope, this);
            this.Focus();
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if(inactiveElement != null)
            {
                valueWrapper.Child = activeElement;
            }
            this.keyWrapper.Background = bgHighlightColor;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (inactiveElement != null)
            {
                valueWrapper.Child = inactiveElement;
            }
            this.keyWrapper.Background = bgColor;
            base.OnMouseLeave(e);
        }
        public override string ToString()
        {
            return "PropContainer: " + propertyName;
        }
    }
}
