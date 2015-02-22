using Aegir.ViewModel;
using AegirLib.Data;
using System;
using System.Collections.Generic;
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

namespace Aegir.View
{
    /// <summary>
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar : UserControl
    {
        Point lastMouseDown;
        TreeViewItem draggedItem, targetTreeItem;
        Actor targetActor;

        public Sidebar()
        {
            InitializeComponent();
        }
        private void treeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                lastMouseDown = e.GetPosition(ObjectTree);
            }
        }
        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(ObjectTree);


                    if ((Math.Abs(currentPosition.X - lastMouseDown.X) > 20.0) ||
                        (Math.Abs(currentPosition.Y - lastMouseDown.Y) > 20.0))
                    {
                        draggedItem = (TreeViewItem)sender;
                        Actor selectedItem = ObjectTree.SelectedItem as Actor;

                        if (draggedItem != null && selectedItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(ObjectTree, sender,
                                DragDropEffects.Move);
                            //Checking target is not null and item is dragging(moving)
                            if ((finalDropEffect == DragDropEffects.Move) 
                                && (targetTreeItem != null)
                                && (targetActor !=null))
                            {
                                // A Move drop was accepted
                                CopyItem(selectedItem, targetActor);
                                targetTreeItem = null;
                                draggedItem = null;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Error");
            }
        }
        //private void treeView_DragOver(object sender, DragEventArgs e)
        //{
        //    try
        //    {

        //        Point currentPosition = e.GetPosition(ObjectTree);


        //        if ((Math.Abs(currentPosition.X - lastMouseDown.X) > 10.0) ||
        //            (Math.Abs(currentPosition.Y - lastMouseDown.Y) > 10.0))
        //        {
        //            // Verify that this is a valid drop and then store the drop target
        //            TreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement);
        //            if (CheckDropTarget(draggedItem, item))
        //            {
        //                e.Effects = DragDropEffects.Move;
        //            }
        //            else
        //            {
        //                e.Effects = DragDropEffects.None;
        //            }
        //        }
        //        e.Handled = true;
        //    }
        //    catch (Exception)
        //    {
        //        Debug.WriteLine("Error");
        //    }
        //}
        private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                TreeViewItem TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (TargetItem != null && draggedItem != null)
                {
                    targetTreeItem = TargetItem;
                    targetActor = targetTreeItem.DataContext as Actor;
                    e.Effects = DragDropEffects.Move;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Error");
            }



        }

        private void CopyItem(Actor item, Actor to)
        {
            //no need to make a big fuss about it if we drop on existing parent
            if (item.Parent == to) return;
            //Ignore add to self
            if (item == to) return;

            if (!CheckIsRelated(item,to))
            {
                //Asking user wether he want to drop the dragged TreeViewItem here or not
                if (MessageBox.Show("Would you like to drop " + item.Name + " into " + to.Name + "", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        to.AddChildActor(item);
                        IActorContainer parent = item.Parent;
                        parent.RemoveActor(item);
                        item.Parent = to;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Error");
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid Add For  " + item.Name +" to "+to.Name);
            }

        }
        /// <summary>
        /// We want to check that the target node is not a child of us 
        /// (Effectively making that child parent of its E.G grandparent).
        /// </summary>
        /// <param name="toMove"></param>
        /// <param name="toTarget"></param>
        /// <returns></returns>
        private bool CheckIsRelated(IActorContainer toMove, IActorContainer toTarget)
        {
            IActorContainer parentToCheck = toTarget.Parent;
            while(parentToCheck != null)
            {
                if(parentToCheck == toMove)
                {
                    return true;
                }
                parentToCheck = parentToCheck.Parent;
            }
            return false;
        }
        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }
    }
}
