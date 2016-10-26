using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ViewPropertyGrid.PropertyGrid
{
    /// <summary>
    /// Interaction logic for PropertyGrid.xaml
    /// </summary>
    public partial class PropertyGrid : UserControl
    {
        public const string NoCategoryName = "Misc";
        private ControlFactory controlFactory;

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
            controlFactory = new ControlFactory();
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
            ResetCategories();
            if (newObject != null)
            {
                //Get all properties from the factory
                //This will read the propertyinfo and create our own
                //convienient format (that is also cached)
                InspectableProperty[] properties = DefaultPropertyFactory.GetProperties(newObject);

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

        private void ResetCategories()
        {
            foreach (var category in categoryViews)
            {
                category.Value.Dispose();
            }
            categoryViews.Clear();
        }

        /// <summary>
        /// Clears the UI of properties
        /// </summary>
        private void ClearGridUI()
        {
            this.CategoryPanel.Children.Clear();
        }

        /// <summary>
        /// Sets the state of the propertygrid to a "No selected object"
        /// </summary>
        private void SetEmptyGridUi()
        {
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
            ValueControl valueControl;
            PropertyContainer propContainer;

            //Check if the property has a custom control
            if (propertyMetadata.HasCustomControl)
            {
                valueControl = propertyMetadata.CustomControlFactory.GetControl(property);
            }
            else
            {
                valueControl = controlFactory.GetControl(property);
            }
            //If the edit mode is set to on focus, create a textblock for when its not
            if (valueControl.EditBehaviour == EditingBehaviour.OnFocus)
            {
                TextBlock unfocusElement = controlFactory.CreateUnFocusedTextBlock(property.ReflectionData.Name, property.Target);
                propContainer = new PropertyContainer(propertyMetadata.Name, valueControl.Control, unfocusElement);
            }
            else
            {
                propContainer = new PropertyContainer(propertyMetadata.Name, valueControl.Control);
            }
            container.AddProperty(propContainer);
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
            categoryViews.Add(metaData.Category, newCategory);
            newCategory.Header = metaData.Category;

            CategoryPanel.Children.Add(newCategory);

            return newCategory;
        }

        private void ScrollViewer_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            (e.NewFocus as FrameworkElement)?.BringIntoView();
            Debug.WriteLine("Keyboard Focus" + e.NewFocus.ToString());
        }
    }
}