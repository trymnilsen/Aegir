using Aegir.Rendering.Camera;
using Aegir.Rendering.Visual;
using Aegir.View.Rendering.Tool;
using AegirCore.Behaviour.World;
using HelixToolkit.Wpf;
using log4net;
using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class RendererViewport
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RendererViewport));
        private HelixViewport3D viewport;
        private List<RenderItemListener> listeners;

        private VisualFactory visualFactory;
        private ManipulatorGizmo sceneManipulator;
        private AegirCore.Behaviour.World.Transform followTransform;

        private Vector3D CameraPositionOffset;
        private Vector3D CameraTargetOffset;
        public static Point3D followTransformPoint = new Point3D();

        public HelixViewport3D Viewport
        {
            get { return viewport; }
        }
        public Transform FollowTransform
        {
            get { return followTransform; }
            set
            {
                followTransform = value;
                followTransformPoint = new Point3D(value.LocalPosition.X, value.LocalPosition.Y, value.LocalPosition.Z);
            }
        }

        public VisualFactory VisualFactory
        {
            get
            {
                if (visualFactory == null)
                {
                    string viewPortName = "NAMENOTDEFINED";
                    if (viewport.Name != null && viewport.Name != string.Empty)
                    {
                        viewPortName = viewport.Name;
                    }
                    log.WarnFormat("No visual factory provided for viewport {0}",
                        viewPortName);
                }
                return visualFactory;
            }
            set { visualFactory = value; }
        }

        public RenderingMode RenderMode { get; set; }
        public HelixViewport3D overlay { get; private set; }

        public RendererViewport(HelixViewport3D sceneViewport)
        {
            RenderMode = RenderingMode.Solid;
            this.viewport = sceneViewport;
            this.visualFactory = VisualFactory;
            listeners = new List<RenderItemListener>();
        }

        public void AddVisual(Visual3D visual, AegirCore.Behaviour.World.Transform transform)
        {
            RenderItemListener listener = new RenderItemListener(visual, transform);
            listeners.Add(listener);
            viewport.Children.Add(visual);
        }

        public SceneActor ResolveRenderItem(Visual3D visual)
        {
            return VisualFactory?.GetRenderItem(RenderMode, visual);
        }

        public void AddDummy(AegirCore.Behaviour.World.Transform transformBehaviour)
        {
            Visual3D dummyVisual = VisualFactory.GetDummyVisual();
            AddVisual(dummyVisual, transformBehaviour);
        }

        public void InvalidateVisuals()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].Invalidate();
            }

            if (FollowTransform != null)
            {
                CameraController.RotateOrigin = new Point3D(followTransform.LocalPosition.X, followTransform.LocalPosition.Y, followTransform.LocalPosition.Z);
            }
        }

        public void CalculateFollowCameraOffset(AegirCore.Behaviour.World.Transform followTransform)
        {
            viewport.Dispatcher.Invoke(() =>
            {
                this.followTransform = followTransform;
                AegirType.Vector3 fp = followTransform.LocalPosition;
                CameraPositionOffset = new Vector3D(fp.X, fp.Y, fp.Z) - (Vector3D)viewport.CameraController.CameraPosition;
            });
        }

        private void DoCameraFollow()
        {
            AegirType.Vector3 fp = followTransform.LocalPosition;
            viewport.Dispatcher.Invoke(() =>
            {
                viewport.CameraController.CameraPosition = CameraPositionOffset + new Point3D(fp.X, fp.Y, fp.Z);
                viewport.CameraController.CameraTarget = new Point3D(fp.X, fp.Y, fp.Z);
            });
        }

        public void AddRenderItemToView(SceneActor renderItem)
        {
            if (VisualFactory == null)
            {
                string viewPortName = "NAMENOTDEFINED";
                if (viewport.Name != null && viewport.Name != string.Empty)
                {
                    viewPortName = viewport.Name;
                }
                log.WarnFormat("No visual factory provided for viewport {0}",
                    viewPortName);
            }
            else
            {
                Visual3D visual = VisualFactory.GetVisual(RenderMode, renderItem);
                AddVisual(visual, renderItem.Transform);
            }
        }

        public void ClearView()
        {
            foreach (RenderItemListener listener in listeners)
            {
                listener.Dispose();
            }
            viewport.Children.Clear();
        }
    }
}