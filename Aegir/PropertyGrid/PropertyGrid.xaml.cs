using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Aegir.PropertyGrid
{
    /// <summary>
    /// Interaction logic for PropertyGrid.xaml
    /// </summary>
    public partial class PropertyGrid : UserControl
    {
        /// <summary>
        /// We keep track of the objects we are currently listing to so we
        /// can unsub from the later
        /// </summary>
        private Dictionary<int, WeakReference<INotifyPropertyChanged>> eventPublishers;
        /// <summary>
        /// Name of properties which would yield a reload
        /// </summary>
        private HashSet<string> requiresListReload;

        private Dictionary<string, CategoryContainer> categoryViews;
        /// <summary>
        /// The current properties we are able to edit and is shown in the UI
        /// </summary>
        private InspectablePropertyMetadata[] currentProperties;

        // Using a DependencyProperty as the backing store for SelectedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register(nameof(SelectedObject), typeof(object), typeof(PropertyGrid), new PropertyMetadata(SelectedObjectChanged));


        /// <summary>
        /// The object that is editable in the grid
        /// </summary>
        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set
            {
                SetValue(SelectedObjectProperty, value);
            }
        }
        /// <summary>
        /// Creates an inits a new Propertygrid
        /// </summary>
        public PropertyGrid()
        {
            categoryViews = new Dictionary<string, CategoryContainer>();
            requiresListReload = new HashSet<string>();
            eventPublishers = new Dictionary<int, WeakReference<INotifyPropertyChanged>>();
            InitializeComponent();
        }
        /// <summary>
        /// Disposes the propertygrid
        /// </summary>
        public void Dispose()
        {
            Cleanup();
        }
        /// <summary>
        /// Update the Grid with a new target
        /// </summary>
        /// <param name="oldObject">the previous object edited</param>
        /// <param name="newObject">the new object being edited</param>
        private void UpdatePropertyGridTarget(object oldObject, object newObject)
        {
            Cleanup();
            ClearGridUI();
            if (newObject != null)
            {
                //Get all properties from the factory
                //This will read the propertyinfo and create our own
                //convienient format (that is also cached)
                InspectableProperty[] properties = GetProperties(newObject);

                foreach (InspectableProperty property in properties)
                {
                    InspectablePropertyMetadata propertiyMetadata =
                        DefaultPropertyFactory.GetPropertyMetadata(property);

                    if (property.Target is INotifyPropertyChanged)
                    {
                        var target = property.Target as INotifyPropertyChanged;
                        if (!eventPublishers.ContainsKey(target.GetHashCode()))
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
        /// <summary>
        /// Clears the UI of properties
        /// </summary>
        private void ClearGridUI()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Sets the state of the propertygrid to a "No selected object"
        /// </summary>
        private void SetEmptyGridUi()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Cleans up the grid, removing events etc
        /// </summary>
        private void Cleanup()
        {
            //Remove all events
            foreach (var eventPub in eventPublishers.Values)
            {
                INotifyPropertyChanged pubInstance;
                if (eventPub.TryGetTarget(out pubInstance))
                {
                    pubInstance.PropertyChanged -= TargetObject_PropertyChanged;
                }
            }
            eventPublishers.Clear();
        }


        private InspectableProperty[] GetProperties(object newObject)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Callback for when a property on the object(s) we are editing changes
        /// via the INotifyPropertyChanged event
        /// </summary>
        /// <param name="sender">The instance the event was triggered on</param>
        /// <param name="e">PropertyChanged Eventargs containing the name of our property</param>
        private void TargetObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (requiresListReload.Contains(e.PropertyName))
            {

            }
        }
        /// <summary>
        /// Clears any property editing UI elements from the grid
        /// </summary>
        private void ClearPropertyGrid()
        {

        }
        /// <summary>
        /// Callback for the selected object dependency property changed
        /// Triggers the UpdatePropertyGridTarget of the propertygrid view
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void SelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyGrid view = d as PropertyGrid;
            view?.UpdatePropertyGridTarget(e.OldValue, e.NewValue);
        }
        /// <summary>
        /// Adds a property to the UI
        /// </summary>
        /// <param name="property"></param>
        /// <param name="propertyMetadata"></param>
        private void AddProperty(InspectableProperty property, InspectablePropertyMetadata propertyMetadata)
        {
            //Get the category of the property
            CategoryContainer container = GetCategory(propertyMetadata);

        }
        /// <summary>
        /// Removes a property from the UI
        /// </summary>
        /// <param name="property"></param>
        private void RemoveProperty(InspectablePropertyMetadata property)
        {

        }
        private CategoryContainer GetCategory(InspectablePropertyMetadata metaData)
        {
            if (categoryViews.ContainsKey(metaData.Category))
            {
                return categoryViews[metaData.Category];
            }

            CategoryContainer newCategory = new CategoryContainer();
            newCategory.Header = metaData.Category;

            CategoryPanel.Children.Add(newCategory);

            return newCategory;
        }
    }
}
