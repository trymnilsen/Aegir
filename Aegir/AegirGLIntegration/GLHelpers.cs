using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace OpenGL
{
    /// <summary> Random methods. </summary>
    public static class GLHelpers
    {
        /// <summary>Clear the GL scene</summary>
        /// <param name="color">The color of the background</param>
        public static void ClearColor(System.Windows.Media.Color color)
        {
            GL.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
        /// <summary>Set the GL forecolor</summary>
        /// <param name="color">The color value</param>
        public static void Color4(System.Windows.Media.Color color)
        {
            //GL.Color4(color.R, color.G, color.B, color.A);
        }

        /// <summary>Make a cube with normals and 2D texture coordinates</summary>
        /// <param name="width"></param>
        public static void Cube2D(double width)
        {
            //var w = width / 2;
            //using (GLDraw.Begin(BeginMode.Quads))
            //{
            //    // back face
            //    GL.Normal3(0, 0, -w); GL.TexCoord2(1, 1); GL.Vertex3(w, w, -w);
            //    GL.Normal3(0, 0, -w); GL.TexCoord2(1, 0); GL.Vertex3(w, -w, -w);
            //    GL.Normal3(0, 0, -w); GL.TexCoord2(0, 0); GL.Vertex3(-w, -w, -w);
            //    GL.Normal3(0, 0, -w); GL.TexCoord2(0, 1); GL.Vertex3(-w, w, -w);
            //    // bottom face
            //    GL.Normal3(0, -w, 0); GL.TexCoord2(1, 1); GL.Vertex3(w, -w, w);
            //    GL.Normal3(0, -w, 0); GL.TexCoord2(1, 0); GL.Vertex3(-w, -w, w);
            //    GL.Normal3(0, -w, 0); GL.TexCoord2(0, 0); GL.Vertex3(-w, -w, -w);
            //    GL.Normal3(0, -w, 0); GL.TexCoord2(0, 1); GL.Vertex3(w, -w, -w);
            //    // top face
            //    GL.Normal3(0, w, 0); GL.TexCoord2(1, 1); GL.Vertex3(w, w, w);
            //    GL.Normal3(0, w, 0); GL.TexCoord2(1, 0); GL.Vertex3(-w, w, w);
            //    GL.Normal3(0, w, 0); GL.TexCoord2(0, 0); GL.Vertex3(-w, w, -w);
            //    GL.Normal3(0, w, 0); GL.TexCoord2(0, 1); GL.Vertex3(w, w, -w);
            //    // left face
            //    GL.Normal3(-w, 0, 0); GL.TexCoord2(1, 1); GL.Vertex3(-w, w, w);
            //    GL.Normal3(-w, 0, 0); GL.TexCoord2(1, 0); GL.Vertex3(-w, w, -w);
            //    GL.Normal3(-w, 0, 0); GL.TexCoord2(0, 0); GL.Vertex3(-w, -w, -w);
            //    GL.Normal3(-w, 0, 0); GL.TexCoord2(0, 1); GL.Vertex3(-w, -w, w);
            //    // right face
            //    GL.Normal3(w, 0, 0); GL.TexCoord2(1, 1); GL.Vertex3(w, w, w);
            //    GL.Normal3(w, 0, 0); GL.TexCoord2(1, 0); GL.Vertex3(w, w, -w);
            //    GL.Normal3(w, 0, 0); GL.TexCoord2(0, 0); GL.Vertex3(w, -w, -w);
            //    GL.Normal3(w, 0, 0); GL.TexCoord2(0, 1); GL.Vertex3(w, -w, w);
            //    // front face
            //    GL.Normal3(0, 0, w); GL.TexCoord2(1, 1); GL.Vertex3(w, w, w);
            //    GL.Normal3(0, 0, w); GL.TexCoord2(1, 0); GL.Vertex3(w, -w, w);
            //    GL.Normal3(0, 0, w); GL.TexCoord2(0, 0); GL.Vertex3(-w, -w, w);
            //    GL.Normal3(0, 0, w); GL.TexCoord2(0, 1); GL.Vertex3(-w, w, w);
            //}
        }
        public static void PlaneMesh(float width, float height, int meshSize, float z)
        {
            float x = -width;
            float y = height;
            float dX = width * 2f / meshSize;
            float dY = -height * 2f / meshSize;

            //GL.Normal3(0, 0, 1);
            //for (int i = 0; i < meshSize; i++)
            //{
            //    y = height;
            //    using (GLDraw.Begin(BeginMode.QuadStrip))
            //    {
            //        for (int j = 0; j < meshSize + 1; j++)
            //        {
            //            GL.TexCoord2(i / (float)meshSize, j / (float)meshSize); GL.Vertex3(x, y, z);
            //            GL.TexCoord2((i + 1) / (float)meshSize, j / (float)meshSize); GL.Vertex3(x + dX, y, z);
            //            y += dY;
            //        }
            //    }
            //    x += dX;
            //}
        }
        public static void Cube()
        {
            Cube(-.25f, -.25f, -.25f, .25f, .25f, .25f);
        }
        public static void Cube(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            //using (GLDraw.Begin(BeginMode.Quads))
            //{
            //    GLHelpers.Color4(Colors.Silver);
            //    GL.Vertex3(x1, y1, z1);
            //    GL.Vertex3(x1, y2, z1);
            //    GL.Vertex3(x2, y2, z1);
            //    GL.Vertex3(x2, y1, z1);

            //    GLHelpers.Color4(Colors.Honeydew);
            //    GL.Vertex3(x1, y1, z1);
            //    GL.Vertex3(x2, y1, z1);
            //    GL.Vertex3(x2, y1, z2);
            //    GL.Vertex3(x1, y1, z2);

            //    GLHelpers.Color4(Colors.Moccasin);
            //    GL.Vertex3(x1, y1, z1);
            //    GL.Vertex3(x1, y1, z2);
            //    GL.Vertex3(x1, y2, z2);
            //    GL.Vertex3(x1, y2, z1);

            //    GLHelpers.Color4(Colors.SkyBlue);
            //    GL.Vertex3(x1, y1, z2);
            //    GL.Vertex3(x2, y1, z2);
            //    GL.Vertex3(x2, y2, z2);
            //    GL.Vertex3(x1, y2, z2);

            //    GLHelpers.Color4(Colors.PaleVioletRed);
            //    GL.Vertex3(x1, y2, z1);
            //    GL.Vertex3(x1, y2, z2);
            //    GL.Vertex3(x2, y2, z2);
            //    GL.Vertex3(x2, y2, z1);

            //    GLHelpers.Color4(Colors.ForestGreen);
            //    GL.Vertex3(x2, y1, z1);
            //    GL.Vertex3(x2, y2, z1);
            //    GL.Vertex3(x2, y2, z2);
            //    GL.Vertex3(x2, y1, z2);
            //}
        }
        public static void CubeOutline(float radius, float thickness)
        {
            float t = thickness;
            float a = -radius;
            float b = -radius + t;
            float c = radius - t;
            float d = radius;

            // back
            Cube(a, a, a, d, b, b);
            Cube(a, a, a, b, d, b);
            Cube(a, c, a, d, d, b);
            Cube(c, a, a, d, d, b);

            // front
            Cube(a, a, c, d, b, d);
            Cube(a, a, c, b, d, d);
            Cube(a, c, c, d, d, d);
            Cube(c, a, c, d, d, d);

            // sides
            Cube(a, a, a, b, b, d);
            Cube(c, a, a, d, b, d);
            Cube(a, c, a, b, d, d);
            Cube(c, c, a, d, d, d);
        }
        public static void Plane(double width_2, double height_2, double textureStretch)
        {
            float t = (float)textureStretch;

            //using (GLDraw.Begin(BeginMode.Quads))
            //{
            //    // front face
            //    GL.Normal3(0, 0, 1);
            //    GL.TexCoord2(1 + t, 1 + t); GL.Vertex3(width_2, height_2, 0);
            //    GL.TexCoord2(1 + t, -t); GL.Vertex3(width_2, -height_2, 0);
            //    GL.TexCoord2(-t, -t); GL.Vertex3(-width_2, -height_2, 0);
            //    GL.TexCoord2(-t, 1 + t); GL.Vertex3(-width_2, height_2, 0);
            //}
        }
        public static void Plane(double width_2, double height_2)
        {
            //using (GLDraw.Begin(BeginMode.Quads))
            //{
            //    // front face
            //    GL.Normal3(0, 0, 1);
            //    GL.TexCoord2(1, 0); GL.Vertex3(width_2, height_2, 0);
            //    GL.TexCoord2(1, 1); GL.Vertex3(width_2, -height_2, 0);
            //    GL.TexCoord2(0, 1); GL.Vertex3(-width_2, -height_2, 0);
            //    GL.TexCoord2(0, 0); GL.Vertex3(-width_2, height_2, 0);
            //}
        }
        public static void Sphere(double r, int lats, int longs, Vector3 center)
        {
            //int i, j;
            //for (i = 0; i <= lats; i++)
            //{
            //    double lat0 = Math.PI * (-0.5 + (double)(i - 1) / lats);
            //    double z0 = r * Math.Sin(lat0);
            //    double zr0 = r * Math.Cos(lat0);

            //    double lat1 = Math.PI * (-0.5 + (double)i / lats);
            //    double z1 = r * Math.Sin(lat1);
            //    double zr1 = r * Math.Cos(lat1);

            //    using (GLDraw.Begin(BeginMode.QuadStrip))
            //    {
            //        for (j = 0; j <= longs; j++)
            //        {
            //            double lng = 2 * Math.PI * (double)(j - 1) / longs;
            //            double x = Math.Cos(lng);
            //            double y = Math.Sin(lng);

            //            GL.Normal3(x * zr0, y * zr0, z0);
            //            GL.Vertex3(x * zr0 + center.X, y * zr0 + center.Y, z0 + center.Z);
            //            GL.Normal3(x * zr1, y * zr1, z1);
            //            GL.Vertex3(x * zr1 + center.X, y * zr1 + center.Y, z1 + center.Z);
            //        }
            //    }
            //}
        }
        public static void SphereTextured(double r, int lats, int longs, Vector3 center)
        {
            for (var i = 0; i <= lats; i++)
            {
                double lat0 = Math.PI * (-0.5 + (double)(i - 1) / lats);
                double z0 = r * Math.Sin(lat0);
                double zr0 = r * Math.Cos(lat0);

                double lat1 = Math.PI * (-0.5 + (double)i / lats);
                double z1 = r * Math.Sin(lat1);
                double zr1 = r * Math.Cos(lat1);

                //using (GLDraw.Begin(BeginMode.Quads))
                //{
                //    for (var j = 0; j <= longs; j++)
                //    {
                //        double lng = 2 * Math.PI * (double)(j - 1) / longs;
                //        double x = Math.Cos(lng);
                //        double y = Math.Sin(lng);

                //        GL.TexCoord2((i * 2f) / (lats * 2 + 2), (j * 2f) / (longs * 2 + 2));
                //        GL.Normal3(x * zr0, y * zr0, z0);
                //        GL.Vertex3(x * zr0 + center.X, y * zr0 + center.Y, z0 + center.Z);
                //        GL.TexCoord2((i * 2f + 1) / (lats * 2 + 2), (j * 2f) / (longs * 2 + 2));
                //        GL.Normal3(x * zr1, y * zr1, z1);
                //        GL.Vertex3(x * zr1 + center.X, y * zr1 + center.Y, z1 + center.Z);
                //    }
                //}
            }
        }

        public static Vector3 Add(this Vector3 vector3, System.Windows.Media.Media3D.Point3D point3D)
        {
            vector3.X += (float)point3D.X;
            vector3.Y += (float)point3D.Y;
            vector3.Z += (float)point3D.Z;
            return vector3;
        }
        public static Vector3 ToVector3(this System.Windows.Media.Media3D.Point3D value)
        {
            return new Vector3((float)value.X, (float)value.Y, (float)value.Z);
        }
        public static void ToVector3(this System.Windows.Media.Media3D.Point3D value, out Vector3 vec3)
        {
            vec3.X = (float)value.X;
            vec3.Y = (float)value.Y;
            vec3.Z = (float)value.Z;
        }
        public static Point3D ToPoint3D(this Vector3 vec3)
        {
            return new Point3D(vec3.X, vec3.Y, vec3.Z);
        }
        public static int CompareTo(this Vector3 vector3, Vector3 other)
        {
            int ret = other.Z.CompareTo(vector3.Z);
            if (ret == 0)
                ret = other.Y.CompareTo(vector3.Y);
            if (ret == 0)
                ret = other.X.CompareTo(vector3.X);
            return ret;
        }
        public static Vector3 NextVector3(this Random rand, float min, float max)
        {
            return new Vector3(
                (float)rand.NextDouble() * (max - min) + min,
                (float)rand.NextDouble() * (max - min) + min,
                (float)rand.NextDouble() * (max - min) + min);
        }

        public static bool IsZero(this Box2 box2)
        {
            return box2.Height == 0 && box2.Width == 0;
        }
    }
}
