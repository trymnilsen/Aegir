using AegirLib.Data.Actors;
using AegirSimulation.Simulation.Transformation;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Actors
{
    public class ShipViewModel : ViewModelBase
    {

        private string name;
        private float width;
        private float height;
        private Ship model;

        [Category("General Info")]
        [Description("Name of the ship we are simulating")]
        public string Name
        {
            get { return name; }
            set 
            {
                if(name!=value)
                {
                    name = value; 
                    RaisePropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Backing store for TransformationMode property.
        /// </summary>
        private ETransformationType transformMode;
        /// <summary>
        /// Gets and sets the value of TransformationMode, Also Raises a RaiseProperyChanged Event
        /// Does not update if the value is the same
        /// </summary>
        

        [Category("Transform")]
        [DisplayName("Transformation Mode")]
        [Description("Sets if the transformation should be considered absolute to the world space or relative to its possible parent")]
        public ETransformationType TransformationMode
        {
            get { return transformMode; }
            set
            {
                if (transformMode != value)
                {
                    transformMode = value;
                    RaisePropertyChanged("TransformationMode");
                }
            }
        }
        
        /// <summary>
        /// Backing store for X property.
        /// </summary>
        private double pos_x;
        /// <summary>
        /// Gets and sets the value of X, Also Raises a RaiseProperyChanged Event
        /// Does not update if the value is the same
        /// </summary>
        [Category("Transform")]
        public double X
        {
            get { return pos_x; }
            set
            {
                if (pos_x != value)
                {
                    pos_x = value;
                    RaisePropertyChanged("X");
                }
            }
        }

        /// <summary>
        /// Backing store for Y property.
        /// </summary>
        private double pos_Y;
        /// <summary>
        /// Gets and sets the value of Y, Also Raises a RaiseProperyChanged Event
        /// Does not update if the value is the same
        /// </summary>
        [Category("Transform")]
        public double Y
        {
            get { return pos_Y; }
            set
            {
                if (pos_Y != value)
                {
                    pos_Y = value;
                    RaisePropertyChanged("Y");
                }
            }
        }

        /// <summary>
        /// Backing store for Z property.
        /// </summary>
        private double pos_Z;
        /// <summary>
        /// Gets and sets the value of Z, Also Raises a RaiseProperyChanged Event
        /// Does not update if the value is the same
        /// </summary>
        [Category("Transform")]
        public double Z
        {
            get { return pos_Z; }
            set
            {
                if (pos_Z != value)
                {
                    pos_Z = value;
                    RaisePropertyChanged("Z");
                }
            }
        }

        /// <summary>
        /// Backing store for RotX property.
        /// </summary>
        private double rot_X;
        /// <summary>
        /// Gets and sets the value of RotX, Also Raises a RaiseProperyChanged Event
        /// Does not update if the value is the same
        /// </summary>
        [Category("Transform")]
        [DisplayName("Rotation X")]
        public double RotX
        {
            get { return rot_X; }
            set
            {
                if (rot_X != value)
                {
                    rot_X = value;
                    RaisePropertyChanged("RotX");
                }
            }
        }

        /// <summary>
        /// Backing store for RotY property.
        /// </summary>
        private double rot_Y;
        /// <summary>
        /// Gets and sets the value of RotY, Also Raises a RaiseProperyChanged Event
        /// Does not update if the value is the same
        /// </summary>
        [Category("Transform")]
        [DisplayName("Rotation Y")]
        public double RotY
        {
            get { return rot_Y; }
            set
            {
                if (rot_Y != value)
                {
                    rot_Y = value;
                    RaisePropertyChanged("RotY");
                }
            }
        }

        /// <summary>
        /// Backing store for RotZ property.
        /// </summary>
        private double rot_Z;
        /// <summary>
        /// Gets and sets the value of RotZ, Also Raises a RaiseProperyChanged Event
        /// Does not update if the value is the same
        /// </summary>
        [Category("Transform")]
        [DisplayName("Rotation Z")]
        public double RotZ
        {
            get { return rot_Z; }
            set
            {
                if (rot_Z != value)
                {
                    rot_Z = value;
                    RaisePropertyChanged("RotZ");
                }
            }
        }

        /// <summary>
        /// Backing store for EnginePower property.
        /// </summary>
        private double enginePower;
        /// <summary>
        /// Gets and sets the value of EnginePower, Also Raises a RaiseProperyChanged Event
        /// Does not update if the value is the same
        /// </summary>
        [Category("Simulation")]
        public double EnginePower
        {
            get { return enginePower; }
            set
            {
                if (enginePower != value)
                {
                    enginePower = value;
                    RaisePropertyChanged("EnginePower");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Category("General Info")]
        public float Width
        {
            get { return width; }
            set 
            {
                if(width != value)
                {
                    width = value;
                    RaisePropertyChanged("Width");
                }
            }
        }
        
        [Category("General Info")]
        public float Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    RaisePropertyChanged("Height");
                }
            }
        }
        public override string ToString()
        {
            return Name == null ? "No Name Set" : Name;
        }
    }
}
