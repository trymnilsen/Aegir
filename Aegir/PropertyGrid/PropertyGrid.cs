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
        /// <summary>
        /// We keep track of the objects we are currently listing to so we
        /// can unsub from the later
        /// </summary>
        private Dictionary<int,WeakReference<INotifyPropertyChanged>> eventPublishers;
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
            eventPublishers = new Dictionary<int,WeakReference<INotifyPropertyChanged>>();
        }
        private void UpdatePropertyGridTarget(object oldObject, object newObject)
        {
            Cleanup();
            ClearGridUI();
            if(newObject != null)
            {
                //Get all properties from the factory
                //This will read the propertyinfo and create our own
                //convienient format (that is also cached)
                InspectableProperty[] properties = GetProperties(newObject);

                foreach(InspectableProperty property in properties)
                {
                    InspectablePropertyMetadata propertiyMetadata = 
                        DefaultPropertyFactory.GetPropertyMetadata(property);

                    if(property.Target is INotifyPropertyChanged)
                    {
                        var target = property.Target as INotifyPropertyChanged;
                        if(!eventPublishers.ContainsKey(target.GetHashCode()))
                        {
                            eventPublishers.Add(target.GetHashCode(), new WeakReference<INotifyPropertyChanged>(target));
                        }
                        target.PropertyChanged += TargetObject_PropertyChanged;
                    }

                    AddProperty(property, propertiyMetadata);
                }

            }
            else
            {
                SetEmptyGridUi();
            }
        }

        private void ClearGridUI()
        {
            throw new NotImplementedException();
        }

        private void SetEmptyGridUi()
        {
            throw new NotImplementedException();
        }

        private void Cleanup()
        {
            //Remove all events
            foreach(var eventPub in eventPublishers.Values)
            {
                INotifyPropertyChanged pubInstance;
                if(eventPub.TryGetTarget(out pubInstance))
                {
                    pubInstance.PropertyChanged -= TargetObject_PropertyChanged;
                }
            }
            eventPublishers.Clear();
        }

        private void AddProperty(InspectableProperty property, InspectablePropertyMetadata propertiyMetadata)
        {
            throw new NotImplementedException();
        }

        private InspectableProperty[] GetProperties(object newObject)
        {
            throw new NotImplementedException();
        }

        private void TargetObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(requiresListReload.Contains(e.PropertyName))
            {

            }
        }
        private void RemoveProperty(InspectablePropertyMetadata property)
        {

        }
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
