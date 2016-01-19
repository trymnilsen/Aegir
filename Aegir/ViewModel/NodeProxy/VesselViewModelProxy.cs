using Aegir.Rendering;
using AegirCore.Behaviour.Simulation;
using AegirCore.Behaviour.Vessel;
using AegirCore.Entity;
using AegirCore.Mesh;
using AegirCore.Mesh.Loader;
using AegirCore.Simulation;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Aegir.ViewModel.NodeProxy
{
    public class VesselViewModelProxy : NodeViewModelProxy
    {
        private VesselNavigationBehaviour navBehaviour;
        private FloatingMesh floatMeshBehaviour;

        [Category("Motion")]
        public double Heading
        {
            get
            {
                return navBehaviour.Heading * (180 / Math.PI);
            }
            set
            {
                navBehaviour.Heading = value * (Math.PI / 180);
                RaisePropertyChanged();
            }
        }

        [Category("Motion")]
        [Editor(typeof(PropertyGridEditorDecimalUpDown), typeof(PropertyGridEditorDecimalUpDown))]
        public double Speed
        {
            get { return navBehaviour.Speed; }
            set
            {
                navBehaviour.Speed = value;
                RaisePropertyChanged();
            }
        }

        [Category("Motion")]
        [DisplayName("Rate Of Turn")]
        public double RateOfTurn
        {
            get { return navBehaviour.RateOfTurn; }
            set
            {
                navBehaviour.RateOfTurn = value;
                RaisePropertyChanged();
            }
        }

        [Category("Simulation")]
        [DisplayName("Simulation Mode")]
        public VesselSimulationMode SimMode
        {
            get { return navBehaviour.SimulationMode; }
            set
            {
                navBehaviour.SimulationMode = value;
                RaisePropertyChanged();
            }
        }

        [Category("Simulation")]
        public float Mass
        {
            get { return floatMeshBehaviour.Mass; }
            set { floatMeshBehaviour.Mass = value; }
        }

        [Category("Simulation")]
        [DisplayName("Hull Model")]
        public string VesselHull
        {
            get { return floatMeshBehaviour.HullModelPath; }
            set
            {
                floatMeshBehaviour.HullModelPath = value;
            }
        }

        private RenderingMode hullRendingMode;

        [Category("Rendering")]
        [DisplayName("Hull Rendering")]
        public RenderingMode HullRenderMode
        {
            get { return hullRendingMode; }
            set { hullRendingMode = value; }
        }

        private Color hullColor;

        [Category("Rendering")]
        [DisplayName("Hull Color")]
        public Color HullColor
        {
            get { return hullColor; }
            set { hullColor = value; }
        }

        private bool renderForces;

        [Category("Rendering")]
        [DisplayName("Render Forces")]
        public bool RenderForces
        {
            get { return renderForces; }
            set { renderForces = value; }
        }

        private LerpMode lerpMode;

        [Category("Dynamics")]
        [DisplayName("User Value Interpolation")]
        public LerpMode LerpMode
        {
            get { return lerpMode; }
            set { lerpMode = value; }
        }


        public VesselViewModelProxy(Vessel vessel)
            : base(vessel)
        {
            navBehaviour = vessel.GetComponent<VesselNavigationBehaviour>();
            floatMeshBehaviour = vessel.GetComponent<FloatingMesh>();
        }

        public override void Invalidate()
        {
            RaisePropertyChanged(nameof(RateOfTurn));
            RaisePropertyChanged(nameof(Heading));
            RaisePropertyChanged(nameof(Speed));
            base.Invalidate();
        }

        private void LoadHullModel(string path)
        {
            //floatMeshBehaviour.HullModelPath = value;
            MeshLoader meshLoader = new MeshLoader();
            try
            {
                if (path == String.Empty)
                {
                    return;
                }
                MeshData mesh = meshLoader.LoadMeshAsync(path);
                ((Vessel)nodeData).HullModel = mesh;
                RaisePropertyChanged(nameof(VesselHull));
            }
            catch (Exception e)
            {
                MessageBox.Show("Loading error" + e.ToString());
            }
        }
    }
}