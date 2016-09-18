using Aegir.Map;
using Aegir.ViewModel.NodeProxy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                new ClickableMenuItem("Undo", ()=> { DW("undo"); }),
                new ClickableMenuItem("Redo", ()=> { DW("Redo"); }),
                new SeperatorMenuItem("Undo/Redo"),
                new ClickableMenuItem("Translate", ()=> { DW("Translate"); }),
                new ClickableMenuItem("Rotate", ()=> { DW("Rotate"); }),
                new ClickableMenuItem("None", ()=> { DW("None"); }),
                new SeperatorMenuItem("Gizmo"),
                new SubMenuItem("Snap", new MenuListItem[]
                {
                    new ClickableMenuItem("Snap to Angle",()=> {DW("SnapAngle"); }),
                    new ClickableMenuItem("Snap to Grid", ()=> {DW("SnapGrid"); }),
                }),
                new SubMenuItem("Transform Delay", new MenuListItem[]
                {
                    new ClickableMenuItem("Immidiately",()=> {DW("Immidiately"); }),
                    new ClickableMenuItem("On Stop", ()=> {DW("OnStop"); }),
                }),
                new SubMenuItem("TransformSpace", new MenuListItem[] 
                {
                    new ClickableMenuItem("Local",()=> {DW("TransformLocal"); }),
                    new ClickableMenuItem("World",()=> {DW("TransformWorld"); })
                }),
                new SeperatorMenuItem("Transform"),
                new SubMenuItem("Camera", new MenuListItem[]
                {
                    new ClickableMenuItem("Inspect",()=> {DW("CameraInspect"); }),
                    new ClickableMenuItem("Follow", ()=> {DW("CameraFollow"); }),
                }),
                new SubMenuItem("Debug", new MenuListItem[]
                {
                    new ClickableMenuItem("Debug Labels", ()=> { MapTileVisual.IsDebugEnabled = !MapTileVisual.IsDebugEnabled; }),
                    new ClickableMenuItem("MapZoomOut", ()=> {DW("MapZoomOut"); }),
                    new ClickableMenuItem("MapZoomIn", ()=> {DW("MapZoomIn"); }),
                    new ClickableMenuItem("MapTranslateOffset", ()=> {DW("MapTranslateOffset"); })
                })
            };
            noContextItems = new MenuListItem[]
            {
                new ClickableMenuItem("Line",()=> {DW("Line"); }),
                new ClickableMenuItem("Rectangle",()=> {DW("Rectangle"); }),
                new ClickableMenuItem("Circle",()=> {DW("Circle"); }),
                new ClickableMenuItem("Polygon",()=> {DW("Polygon"); }),
                new SeperatorMenuItem("Create Shape")
            };
            contextItems = new MenuListItem[]
            {
                new ClickableMenuItem("Delete",()=> {DW("Delete"); }),
                new ClickableMenuItem("Duplicate ",()=> {DW("Duplicate"); }),
                new ClickableMenuItem("Camera Follow",()=> {DW("CameraFollow"); })
            };
        }
        /// <summary>
        /// Debug util method for writing to the debug stream
        /// </summary>
        /// <param name="content"></param>
        private void DW(string content)
        {
            Debug.WriteLine(content+" not yet implemented");
            MenuOptionClicked?.Invoke(content);
        }
        public void SetContextMouseTarget(NodeViewModelProxy target)
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
