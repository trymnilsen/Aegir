using Aegir.CustomControl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Aegir.View.Rendering.Menu
{
    public class SceneContextMenu : ContextMenu
    {
        private MenuList items;

        public MenuList MenuItems
        {
            get { return items; }
            set
            {
                items = value;
                MenuItemsSet();
            }
        }

        public SceneContextMenu()
        {
            Debug.WriteLine("Fooo CONTEXT MENU");
        }

        private void MenuItemsSet()
        {
            MenuItems.MenuChanged += MenuItemsChanged;
        }

        private void MenuItemsChanged(IEnumerable<MenuListItem> items)
        {
            this.Items.Clear();
            RebuildMenuItems(items, this);
        }

        private void RebuildMenuItems(IEnumerable<MenuListItem> items, ItemsControl parent)
        {
            foreach (MenuListItem item in items)
            {
                if (item is ClickableMenuItem)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = item.Header;
                    menuItem.Command = (item as ClickableMenuItem).ClickCommand;
                    menuItem.IsCheckable = (item as ClickableMenuItem).Toggle;
                    parent.Items.Add(menuItem);
                }
                else if (item is SeperatorMenuItem)
                {
                    HeaderedSeparator menuItem = new HeaderedSeparator();
                    menuItem.Header = menuItem.Header;
                    parent.Items.Add(menuItem);
                }
                else if (item is SubMenuItem)
                {
                    SubMenuItem subMenuItem = item as SubMenuItem;
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = item.Header;
                    parent.Items.Add(menuItem);
                    RebuildMenuItems(subMenuItem.Children, menuItem);
                }
            }
        }
    }
}