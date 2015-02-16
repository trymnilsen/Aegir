using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Input
{
    public class VirtualTrackball
    {
        private Vector3 pointOnSphereBegin;
        private Quaternion quaternion_old;
        private Quaternion quaternion_new;
        private float mouseVelocity;

        /// <summary>
        /// Window / or element width
        /// </summary>
        public int Window_W { get; set; }
        /// <summary>
        /// Window / or element height
        /// </summary>
        public int Window_H { get; set; }
        /// <summary>
        /// Represents if the trackball is currently beeing rotated by the mouse
        /// </summary>
        public bool Rotating { get; private set; }
        /// <summary>
        /// Initalize a new trackball instance
        /// </summary>
        public VirtualTrackball()
        {

        }
        /// <summary>
        /// Called when we click the mouse on screen. Finds and
        /// sets rotate_begin_vec to be the vector from the origin
        /// to the closest point on the unit sphere.
        /// </summary>
        /// <param name="x">inital mouse position</param>
        /// <param name="y">initial mouse position</param>
        public void RotateBegin(int x, int y)
        {
            Rotating = true;
            pointOnSphereBegin = getClosestPointOnUnitSphere(x, y);
        }
        /// <summary>
        /// Resets the state of the move quaternion
        /// </summary>
        public void RotateEnd()
        {
            Rotating = false;
            quaternion_old = quaternion_new;
        }
        /// <summary>
        /// Move the trackball with the given mouse coordinates
        /// </summary>
        /// <param name="x">Absolute mouse coordinate</param>
        /// <param name="y">Absolute mouse coordinate</param>
        /// <returns>New matrix to transform our camera with</returns>
        /// <remarks>Does nothing if RotateBegin has not been called</remarks>
        public Matrix4 Rotate(int x, int y)
        {
            if (!Rotating) return Matrix4.CreateFromQuaternion(quaternion_old);

            Vector3 pointOnSphereNow = getClosestPointOnUnitSphere(x, y);
            float sphereDotResult = Vector3.Dot(pointOnSphereBegin.Normalized(),
                                                pointOnSphereNow.Normalized());

            float theta = (float)Math.Acos((double)sphereDotResult);
            Vector3 axisOfRotation = Vector3.Cross(pointOnSphereBegin, pointOnSphereNow);
            //As we scroll or roll our trackball we want the rotation axis to change... IE if we rollthe "trackball" 
            //towards us/down we want the model to roll towards us not matter what its current orientation is
            axisOfRotation = Vector3.Transform(axisOfRotation,quaternion_old);
            Quaternion newRotation = Quaternion.FromAxisAngle(axisOfRotation, theta);
            quaternion_new = quaternion_new * newRotation;
            quaternion_new.Normalize();

            //Return our new orientation
            return Matrix4.CreateFromQuaternion(quaternion_new);
        }
        /// <summary>
        /// Returns the normalized (x=[-0.5,0.5], y=[-0.5,0.5]) window 
        /// coordinates from absolute window coordinates.
        /// </summary>
        /// <remarks>
        /// We flip the Y axis
        /// </remarks>
        /// <param name="x">abosolute X window coordinate</param>
        /// <param name="y">absolute Y window coordinate</param>
        /// <returns>normalized window coordinates</returns>
        private Vector2 GetNormalizedWindowCoordinates(int x, int y)
        {
            float nx = (float)x / (float)Window_W - 0.5f;
            float ny = 0.5f - (float)y / (float)Window_H;

            return new Vector2(nx, ny);
        }
        /// <summary>
        /// Computes the closes 3d point on the unit sphere from our
        /// normalized window pos
        /// </summary>
        /// <param name="x">normalized window x position</param>
        /// <param name="y">normalized window y position</param>
        /// <returns></returns>
        private Vector3 getClosestPointOnUnitSphere(int x, int y)
        {
            Vector3 point = new Vector3();
            Vector2 normCoords = GetNormalizedWindowCoordinates(x, y);
            float k = normCoords.Length;

            if(k>=0.5)
            {
                point.X = normCoords.X / k;
                point.Y = normCoords.Y / k;
            }
            else
            {
                //Only Calculate if it will yield a useable result
                point.X = 2 * normCoords.X;
                point.Y = 2 * normCoords.Y;
                point.Z = (float)Math.Sqrt(1 - 4 * k * k);
            }

            return point;

        }
    }
}
