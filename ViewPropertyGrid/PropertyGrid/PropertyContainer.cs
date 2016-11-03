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
using ViewPropertyGrid.Util;

namespace ViewPropertyGrid.PropertyGrid
{
    public class PropertyContainer : Grid, IDisposable
    {
        private readonly SolidColorBrush bgColor = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush bgHighlightColor = new SolidColorBrush(Colors.LightBlue);

        private TextBlock textLabel;
        private ValueControl activeElement;
        private FrameworkElement inactiveElement;
        private Border keyWrapper;
        private Border valueWrapper;
        public string propertyName;
        private bool IsEditing;

        public PropertyContainer(string propertyName, ValueControl valueControl)
            : this(propertyName, valueControl, null) { }

        public PropertyContainer(string propertyName, ValueControl valueControl, FrameworkElement inactiveElement)
        {
            this.propertyName = propertyName;
            this.activeElement = valueControl;
            this.inactiveElement = inactiveElement;

            //Add three columns
            ColumnDefinitions.Add(new ColumnDefinition() { });
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
            //If we only have one element (e.g textblock or something, we set the child to this)
            //if we have both an active element and an inactive (E.G textbox for editing and textblock when inactive)
            //set the child to the inactive element
            if (inactiveElement == null)
            {
                valueWrapper.Child = valueControl.Control;
            }
            else
            {
                //Attach click listeners to the active element
                //we do this to not loose focus on the element 
                //if the mouse moves outside this property containers bounds while editing it
 
                valueWrapper.Child = inactiveElement;
                //valueControl.Control.GotKeyboardFocus += ValueControl_GotKeyboardFocus;
            }

            this.Focusable = true;
        }

        //private void ValueControl_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    IsEditing = true;
        //}

        internal void PersistCurrentValue()
        {

        }
        //private void ValueControl_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine($"-- ValueControl lost focus on {propertyName} for ctrl: {sender.ToString()}");
        //    e.Handled = true;
        //}

        //private void ValueControl_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    Debug.WriteLine($"-- ValueControl lost keyboard focus on {propertyName} for ctrl: {sender.ToString()}");
        //}

        //private void ValueControl_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine($"++ ValueControl got focus on {propertyName} for ctrl: {sender.ToString()}");
        //    IsEditing = true;
        //}

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.keyWrapper.Background = bgHighlightColor;
            Debug.WriteLine("GotKeyboardFocus: " + propertyName);
            this.IsEditing = true;
            activeElement.EditStart();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.keyWrapper.Background = bgHighlightColor;
            Debug.WriteLine("GotFocus: " + propertyName);
            
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);

            DependencyObject newFocus = e.NewFocus as DependencyObject;
            if(newFocus!=null)
            {
                PropertyContainer newTargetContainer = newFocus.FindAncestor<PropertyContainer>();
                bool isChildOfPropertyGrid = newFocus.FindAncestor<PropertyGrid>() != null;
                if(isChildOfPropertyGrid)
                {
                    //Persist our value

                    //If we also clicked somewhere else, switch to our inactive element
                    if(newTargetContainer != this)
                    {
                        IsEditing = false;
                        //We are going out of edit mode, resume the continous
                        //binding for our editor
                        activeElement.EditEnd();
                        //We have a inactive element we also need to switch to
                        if (inactiveElement != null)
                        {
                            valueWrapper.Child = inactiveElement;
                        }
                    }
                }
            }
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            this.keyWrapper.Background = bgColor;
            Debug.WriteLine("Lost Focus " + propertyName);
        }

        //protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        //{
        //    //base.OnPreviewLostKeyboardFocus(e);
        //}

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    //if (!IsEditing && valueWrapper.Child == inactiveElement)
        //    //{
        //    //    valueWrapper.Child = activeElement;
        //    //    valueWrapper.Child.Focus();
        //    //}
        //}

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (inactiveElement != null)
            {
                valueWrapper.Child = activeElement.Control;
            }
            this.keyWrapper.Background = bgHighlightColor;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (inactiveElement != null && IsEditing == false)
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

        public void Dispose()
        {
            //if (activeElement != null)
            //{
            //    activeElement.Control.GotKeyboardFocus -= ValueControl_GotKeyboardFocus;
            //}
        }
    }
}