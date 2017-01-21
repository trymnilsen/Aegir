using Aegir.Map;
using Aegir.ViewModel.EntityProxy;
using Aegir.ViewModel.EntityProxy.Node;
using Aegir.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TinyMessenger;

namespace Aegir.View.Rendering.Menu
{
    public class MenuList
    {
        private MenuListItem[] defaultItems;
        private MenuListItem[] noContextItems;
        private MenuListItem[] contextItems;
        private ITinyMessengerHub Messenger { get; set; }

        public MenuList()
        {
            defaultItems = new MenuListItem[]
            {
                new ClickableMenuItem("Translate", ()=> { DW("Translate"); }),
                new ClickableMenuItem("Rotate", ()=> { DW("Rotate"); }),
                new ClickableMenuItem("None", ()=> { DW("None"); }),
                new ClickableMenuItem("Snap To Angle", ()=> {DW("SnapToAngle"); }, true),
                new ClickableMenuItem("Snap To Position", ()=> {DW("Snap To Position"); },true),
                new ClickableMenuItem("Snap Preferences..", ()=> { OpenSnapPreferences(); }),
                new SeperatorMenuItem("Transform"),
                new SubMenuItem("Debug", new MenuListItem[]
                {
                    new ClickableMenuItem("Debug Labels", ()=> { MapTileVisual.IsDebugEnabled = !MapTileVisual.IsDebugEnabled; }),
                    new ClickableMenuItem("MapZoomOut", ()=> {DW("MapZoomOut"); }),
                    new ClickableMenuItem("MapZoomIn", ()=> {DW("MapZoomIn"); }),
                    new ClickableMenuItem("MapTranslateOffset", ()=> {DW("MapTranslateOffset"); })
                }),
                new SeperatorMenuItem("Debug"),
            };
            noContextItems = new MenuListItem[]
            {
                new ClickableMenuItem("Line",()=> {DW("Line"); }),
                new ClickableMenuItem("Point",()=> {DW("Rectangle"); }),
                new ClickableMenuItem("Polygon",()=> {DW("Polygon"); }),
                new SeperatorMenuItem("Create Shape"),
                new ClickableMenuItem("Antenna",()=> {DW("Antenna"); }),
                new ClickableMenuItem("Gyro",()=> {DW("Gyro"); })
            };
            contextItems = new MenuListItem[]
            {
                new ClickableMenuItem("Delete",()=> {DW("Delete"); }),
                new ClickableMenuItem("Duplicate ",()=> {DW("Duplicate"); }),
                new ClickableMenuItem("Camera Follow",()=> {DW("CameraFollow"); })
            };
        }

        private void OpenSnapPreferences()
        {
            SnapSettings snapPrefWindow = new SnapSettings();
            snapPrefWindow.Owner = Application.Current.MainWindow;
            snapPrefWindow.ShowDialog();
        }

        /// <summary>
        /// Debug util method for writing to the debug stream
        /// </summary>
        /// <param name="content"></param>
        private void DW(string content)
        {
            Debug.WriteLine(content + " not yet implemented");
            MenuOptionClicked?.Invoke(content);
        }

        public void SetContextMouseTarget(EntityViewModel target)
        {
            List<MenuListItem> items = new List<MenuListItem>();
            items.AddRange(defaultItems);
            items.AddRange(contextItems);
            items.Add(new SeperatorMenuItem(target.Name));
            MenuChanged?.Invoke(items);
        }

        public void SetNoContextTarget()
        {
            List<MenuListItem> items = new List<MenuListItem>();
            items.AddRange(defaultItems);
            items.AddRange(noContextItems);
            MenuChanged?.Invoke(items);
        }

        public delegate void MenuOptionClickedHandler(string option);

        public event MenuOptionClickedHandler MenuOptionClicked;

        public delegate void MenuChangedHandler(IEnumerable<MenuListItem> items);

        public event MenuChangedHandler MenuChanged;
    }
}