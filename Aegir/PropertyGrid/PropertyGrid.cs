using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Aegir.PropertyGrid
{
    public class PropertyGrid : Grid
    {
        private HashSet<string> requiresListReload;
        private InspectablePropertyMetadata[] currentProperties;
        // Using a DependencyProperty as the backing store for SelectedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register(nameof(SelectedObject), typeof(object), typeof(PropertyGrid), new PropertyMetadata(SelectedObjectChanged));

        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set
            {
                SetValue(SelectedObjectProperty, value);
            }
        }

        public PropertyGrid()
        {
            requiresListReload = new HashSet<string>();
        }
        private void UpdatePropertyGridTarget(object oldObject, object newObject)
        {

            if(newObject != null)
            {
                //Get all properties from the factory
                //This will read the propertyinfo and create our own
                //convienient format (that is also cached)
                InspectablePropertyMetadata[] properties = 
                    DefaultPropertyFactory.GetProperties(newObject);

                //If object is able to change we need to check if it has
                //layout change requirements and listen for changes in general
                if(newObject is INotifyPropertyChanged)
                {
                    foreach(var property in properties.Where(x=>x.UpdateLayoutOnValueChange))
                    {
                        requiresListReload.Add(property.Name);
                    }
                    //We need to listen for propertychanged events to know if we need
                    //to reload list of properties
                    (newObject as INotifyPropertyChanged).PropertyChanged += TargetObject_PropertyChanged;
                }

                RecreatePropertyUI();
            }
            else
            {
                ResetPropertyGrid();
            }
        }

        private void RecreatePropertyUI()
        {
            throw new NotImplementedException();
        }

        private void ResetPropertyGrid()
        {
            throw new NotImplementedException();
        }

        private void TargetObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(requiresListReload.Contains(e.PropertyName))
            {
                InspectablePropertyMetadata[] properties =
                    DefaultPropertyFactory.GetProperties(SelectedObject);


            }
        }
        private void RemoveProperty(InspectablePropertyMetadata property)
        private void CleanupPreviousSelected(object oldObject)
        {
            throw new NotImplementedException();
        }

        private void ClearPropertyGrid()
        {
            throw new NotImplementedException();
        }

        private static void SelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyGrid view = d as PropertyGrid;
            view?.UpdatePropertyGridTarget(e.OldValue,e.NewValue);
        }

    }
}
