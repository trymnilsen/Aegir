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
using ViewPropertyGrid.PropertyGrid.Component;
using ViewPropertyGrid.Util;

namespace ViewPropertyGrid.PropertyGrid
{
    /// <summary>
    /// Interaction logic for PropertyGrid.xaml
    /// </summary>
    public partial class PropertyGrid : UserControl
    {
        public const string NoCategoryName = "Misc";
        private ControlFactory controlFactory;
        private bool componentMode;
        private Dictionary<IInspectableComponent, ComponentDescriptor> componentDescriptors;

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


        public bool ShowMessageBoxOnError
        {
            get { return (bool)GetValue(ShowMessageBoxOnErrorProperty); }
            set { SetValue(ShowMessageBoxOnErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowMessageBoxOnError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowMessageBoxOnErrorProperty =
            DependencyProperty.Register(nameof(ShowMessageBoxOnError), typeof(bool), typeof(PropertyGrid), new PropertyMetadata(true));



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
            //GotFocus += PropertyGrid_GotFocus;
            //LostFocus += PropertyGrid_LostFocus;
            //LostKeyboardFocus += PropertyGrid_LostKeyboardFocus;
        }

        //private void PropertyGrid_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    Debug.WriteLine($"!\"\" ### --- --- PropertyGrid: LostFocus {e.NewFocus} Source { e.OriginalSource }");
        //}

        //private void PropertyGrid_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine($"### --- PropertyGrid: LostFocus {e.Source} Source { e.OriginalSource }");
        //}

        //private void PropertyGrid_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine($"### PropertyGrid: GotFocus { sender } Source { e.OriginalSource }");
        //    if(e.OriginalSource is DependencyObject)
        //    {
        //        //Look for any parents name of PropertyContainer type
        //        int numOfStepsBeforeBailingOut = 50;
        //        int currentNumOfSteps = 0;
        //        object parent = e.OriginalSource;
        //        PropertyContainer container = (e.OriginalSource as DependencyObject)?.FindAncestor<PropertyContainer>();

        //        if(container!=null)
        //        {
        //            Debug.WriteLine("------ ¤¤ Found ancestor " + container.propertyName);
        //        }
        //        else
        //        {
        //            Debug.WriteLine("------ ¤¤ Did not find ancestor");
        //        }

        //    }
        //}

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
                componentMode = newObject is IComponentContainer;

                //ComponentContainers and PropertyProviders are handled differently with regards to
                //how they are categorizer and put into category containers
                //as well as the possible header of the category container
                if(componentMode)
                {
                    IComponentContainer componentContainer = newObject as IComponentContainer;
                    foreach(IInspectableComponent component in componentContainer.GetInspectableComponents())
                    {
                        AddInspectableComponent(component);
                    }
                    //Listen for component added/removed events
                    componentContainer.ComponentAdded += ComponentContainer_ComponentAdded;
                    //componentContainer.ComponentRemoved
                    //Add the button at the bottom to pick components
                    AddBehaviourButton();
                }
                else
                {
                    foreach (InspectableProperty property in DefaultPropertyFactory.GetProperties(newObject))
                    {
                        InspectablePropertyMetadata propertyMetadata = DefaultPropertyFactory.GetPropertyMetadata(property);
                        CategoryContainer container = GetDefaultHeaderCategoryContainer(propertyMetadata.Category);
                        ListenToPropertyChanged(property);
                        AddProperty(property, propertyMetadata, container);
                    }
                }
            }
            else
            {
                SetEmptyGridUi();
            }
        }

        private void AddInspectableComponent(IInspectableComponent component)
        {
            InspectableProperty[] properties = component.Properties;
            //Add Container independently of any properties
            ComponentDescriptor descriptor = ComponentDescriptorCache.GetDescriptor(component);
            CategoryContainer container = null;
            if (descriptor.Removable) { container = GetComponentCategoryContainer(component, descriptor.Title); }
            else { container = GetDefaultHeaderCategoryContainer(descriptor.Title); }
            foreach (InspectableProperty property in properties)
            {
                InspectablePropertyMetadata propertyMetadata = DefaultPropertyFactory.GetPropertyMetadata(property);

                ListenToPropertyChanged(property);
                AddProperty(property, propertyMetadata, container);
            }
        }

        private void ComponentContainer_ComponentAdded(IInspectableComponent component)
        {
            AddInspectableComponent(component);
        }

        private void AddBehaviourButton()
        {
            componentMode = true;
            Button addBehaviourButton = new Button() { Content = "Add Behaviour" };
            addBehaviourButton.Click += AddBehaviourButton_Click;
            addBehaviourButton.ContextMenu = new ContextMenu();
            addBehaviourButton.ContextMenu.ContextMenuOpening += AddBehaviourButton_ContextMenuOpening;
            ButtonsPanel.Children.Add(addBehaviourButton);
        }

        private void AddBehaviourButton_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            OpenContextMenu(sender);
        }

        private void OpenContextMenu(object sender)
        {
            //Generate the available ones based on the selected object
            IComponentContainer target = SelectedObject as IComponentContainer;
            Button button = sender as Button;
            if (target != null && button != null)
            {
                ComponentDescriptor[] options = target.GetAvailableComponents();
                IInspectableComponent[] behaviours = target.GetInspectableComponents();

                ContextMenu menu = button.ContextMenu;
                menu.Items.Clear();

                //Get them sorted by group
                var groupedOptions = options.GroupBy(item => item.Group,
                    (key, group) => new { Group = key, Components = group });

                foreach (var group in groupedOptions)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = group.Group;
                    menu.Items.Add(menuItem);
                    foreach (var component in group.Components)
                    {
                        //Ignore the ones that cant be removed
                        if (!component.Removable)
                        {
                            continue;
                        }
                        //Change the tooltip based on two cases
                        //If the component already exists on the object and its unique, disable and show "Behaviour already added"
                        //If the component is removable and not unique just show the description of it
                        bool isEnabled = true;
                        string tooltip = string.Empty;

                        //Check if the behaviour already exists
                        if (behaviours.Any(b => options.Any(o => b.GetType() == o.ComponentType)) && component.Unique)
                        {
                            isEnabled = false;
                            tooltip = "Behaviour already added";
                        }
                        else { tooltip = component.Description; }

                        MenuItem child = new MenuItem();
                        child.Header = component.Title;
                        child.IsEnabled = isEnabled;
                        //Setup click handler
                        if (isEnabled)
                        {
                            child.Click += (s, args) => { AddComponentToCurrentTarget(target, component); };
                        }

                        menuItem.Items.Add(child);
                    }
                }
            }
        }

        private void AddComponentToCurrentTarget(IComponentContainer target, ComponentDescriptor component)
        {
            try
            {
                target.AddComponent(component);
            }
            catch(Exception e)
            {
                if (ShowMessageBoxOnError)
                {
                    MessageBox.Show($"An Error Occured:\n{e.ToString()}");
                }
            }
        }

        private void ListenToPropertyChanged(InspectableProperty property)
        {
            var target = property.Target as INotifyPropertyChanged;
            if(target!=null)
            {
                if (!eventPublishers.ContainsKey(target.GetHashCode()))
                {
                    eventPublishers.Add(target.GetHashCode(), new WeakReference<INotifyPropertyChanged>(target));
                }
                target.PropertyChanged += TargetObject_PropertyChanged;
            }
        }

        private void AddBehaviourButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContextMenu contextMenu = button.ContextMenu;
            contextMenu.PlacementTarget = button;
            contextMenu.IsOpen = true;
            OpenContextMenu(button);
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
            CategoryPanel.Children.Clear();
            ButtonsPanel.Children.Clear();
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
        private void AddProperty(InspectableProperty property, InspectablePropertyMetadata propertyMetadata, CategoryContainer container)
        {
            //Get the category of the property if its not set
            //CategoryContainer container = GetCategoryContainer(propertyMetadata);

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
                propContainer = new PropertyContainer(propertyMetadata.Name, valueControl, unfocusElement);
            }
            else
            {
                propContainer = new PropertyContainer(propertyMetadata.Name, valueControl);
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

        private CategoryContainer GetComponentCategoryContainer(IInspectableComponent component, string headerText)
        {
            string uniqueId = component.GetHashCode().ToString();
            if (categoryViews.ContainsKey(uniqueId))
            {
                return categoryViews[uniqueId];
            }

            CategoryContainer newCategory = new CategoryContainer();
            ComponentCategoryHeader header = new ComponentCategoryHeader();
            header.Header = headerText;
            header.RemoveClicked += () =>
              {
                  ComponentRemoved(component);
              };
            newCategory.Header = header;

            categoryViews.Add(uniqueId, newCategory);
            CategoryPanel.Children.Add(newCategory);
            return newCategory;
        }

        private void ComponentRemoved(IInspectableComponent component)
        {
            (SelectedObject as IComponentContainer)?.ComponentRemoved(component);
        }

        private CategoryContainer GetDefaultHeaderCategoryContainer(string headerText)
        {
            if (categoryViews.ContainsKey(headerText))
            {
                return categoryViews[headerText];
            }

            CategoryContainer newCategory = new CategoryContainer();
            categoryViews.Add(headerText, newCategory);
            newCategory.Header = headerText;

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