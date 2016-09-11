using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ViewPropertyGrid.PropertyGrid
{
    public class PropertyContainer : Grid
    {

        private TextBlock textLabel;
        private Grid valuecontent;
        private Border keyWrapper;
        private Border valueWrapper;
        private string propertyName;
        public PropertyContainer(string propertyName)
        {
            this.propertyName = propertyName;
            //Add three columns
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
            ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width=GridLength.Auto,
                    SharedSizeGroup = "PropGridColumn"
                });


            //Create wrappers
            keyWrapper = new Border() { Name = "keywrapper" };
            valueWrapper = new Border() { Name = "valuewrapper" };
            
            Grid.SetColumn(keyWrapper, 0);
            Grid.SetColumn(valueWrapper, 1);

            this.Children.Add(keyWrapper);
            this.Children.Add(valueWrapper);

            textLabel = new TextBlock();
            textLabel.Text = propertyName;
            valuecontent = new Grid();

            keyWrapper.Child = textLabel;
            valueWrapper.Child = valuecontent;

            this.Focusable = true;

        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            Debug.WriteLine("Got keyboard focus"+ propertyName);
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            Debug.WriteLine("Got focus: " + propertyName);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Debug.WriteLine("Mouse down: " + propertyName);
        }
    }
}
