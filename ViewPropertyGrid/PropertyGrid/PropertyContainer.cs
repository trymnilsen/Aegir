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
        public PropertyContainer(string propertyName)
        {
            //Add three columns
            ColumnDefinitions.Add(new ColumnDefinition() { SharedSizeGroup = "PropGridColumn" });
            ColumnDefinitions.Add(new ColumnDefinition());

            textLabel = new TextBlock();
            textLabel.Text = propertyName;
            textLabel.Margin = new Thickness(0, 0, 0, 0);
            Grid.SetColumn(textLabel, 0);
            textLabel.Background = new SolidColorBrush(Colors.White);

            this.Children.Add(textLabel);
            valuecontent = new Grid();
            valuecontent.Margin = new Thickness(1, 0, 0, 0);
            valuecontent.Background = new SolidColorBrush(Colors.White);
            Grid.SetColumn(valuecontent, 1);

            this.Children.Add(valuecontent);
            this.Focusable = true;
            this.Margin = new System.Windows.Thickness(12,0,1,0);

        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            Debug.WriteLine("Got keyboard focus"+ textLabel.Text);
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            Debug.WriteLine("Got focus: " + textLabel.Text);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Debug.WriteLine("Mouse down: " + textLabel.Text);
        }
    }
}
