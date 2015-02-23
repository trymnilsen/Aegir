using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using AegirLib.Data;
using Aegir.Input;

namespace Aegir.Rendering
{
    public class Camera
    {
        private Matrix4 cameraTransform;
        private float posX;
        private float posY;
        private float posZ;
        private bool isDirty;
        private Vector2 viewPort;
        private VirtualTrackball trackball;

        public Matrix4 CameraMatrix
        {
            get
            {
                if (isDirty) { RecalculateCameraTransformation(); }
                return cameraTransform;
            }
            set { cameraTransform = value; }
        }
        
        public float X
        {
            get { return posX; }
            set 
            { 
                posX = value;
                isDirty = true;
            }
        }

        public float Y
        {
            get { return posY; }
            set
            {
                posY = value;
                isDirty = true;
            }
        }

        public float Z
        {
            get { return posZ; }
            set
            {
                posZ = value;
                isDirty = true;
            }
        }

        private Quaternion rotation;

	    public Quaternion Rotation
	    {
		    get { return rotation;}
		    set { rotation = value;}
	    }
	
        public Vector3 Position
        {
            get { return new Vector3(X, Y, Z); }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }
        public Vector2 ViewPortSize
        {
            get { return viewPort; }
            set
            {
                viewPort = value;
                this.trackball.Window_W = (int)value.X;
                this.trackball.Window_H = (int)value.Y;
            }
        }
        public Camera(Vector3 position)
        {
            this.X = position.X;
            this.Y = position.Y;
            this.Z = position.Z;
        }
        public void OrbitAround(ITransformable around)
        {
            Vector3 targetPos = new Vector3(around.X,around.Y,around.Z);
            //Find distance between camera and transform target
            float distance = (targetPos - Position).Length; 
        }
        public void Fly(Vector3 flyDirection)
        {
            Position = Position + Vector3.Transform(flyDirection,Rotation);
        }

        private void RecalculateCameraTransformation()
        {

        }
        
    }
}
