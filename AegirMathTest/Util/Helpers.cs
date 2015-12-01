using AegirType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AegirMathTests.Util
{
    public class MatrixComparer : IEqualityComparer<Matrix>
    {
        static public MatrixComparer Epsilon = new MatrixComparer(0.000001f);

        private readonly float _epsilon;

        private MatrixComparer(float epsilon)
        {
            _epsilon = epsilon;
        }

        public bool Equals(Matrix x, Matrix y)
        {
            return Math.Abs(x.M11 - y.M11) < _epsilon &&
                    Math.Abs(x.M12 - y.M12) < _epsilon &&
                    Math.Abs(x.M13 - y.M13) < _epsilon &&
                    Math.Abs(x.M14 - y.M14) < _epsilon &&
                    Math.Abs(x.M21 - y.M21) < _epsilon &&
                    Math.Abs(x.M22 - y.M22) < _epsilon &&
                    Math.Abs(x.M23 - y.M23) < _epsilon &&
                    Math.Abs(x.M24 - y.M24) < _epsilon &&
                    Math.Abs(x.M31 - y.M31) < _epsilon &&
                    Math.Abs(x.M32 - y.M32) < _epsilon &&
                    Math.Abs(x.M33 - y.M33) < _epsilon &&
                    Math.Abs(x.M34 - y.M34) < _epsilon &&
                    Math.Abs(x.M41 - y.M41) < _epsilon &&
                    Math.Abs(x.M42 - y.M42) < _epsilon &&
                    Math.Abs(x.M43 - y.M43) < _epsilon &&
                    Math.Abs(x.M44 - y.M44) < _epsilon;
        }

        public int GetHashCode(Matrix obj)
        {
            throw new NotImplementedException();
        }
    }

    public class FloatComparer : IEqualityComparer<float>
    {
        static public FloatComparer Epsilon = new FloatComparer(0.000001f);

        private readonly float _epsilon;

        private FloatComparer(float epsilon)
        {
            _epsilon = epsilon;
        }

        public bool Equals(float x, float y)
        {
            return Math.Abs(x - y) < _epsilon;
        }

        public int GetHashCode(float obj)
        {
            throw new NotImplementedException();
        }
    }

    public class BoundingSphereComparer : IEqualityComparer<BoundingSphere>
    {
        static public BoundingSphereComparer Epsilon = new BoundingSphereComparer(0.000001f);

        private readonly float _epsilon;

        private BoundingSphereComparer(float epsilon)
        {
            _epsilon = epsilon;
        }

        public bool Equals(BoundingSphere x, BoundingSphere y)
        {
            return Math.Abs(x.Center.X - y.Center.X) < _epsilon &&
                    Math.Abs(x.Center.Y - y.Center.Y) < _epsilon &&
                    Math.Abs(x.Center.Z - y.Center.Z) < _epsilon &&
                    Math.Abs(x.Radius - y.Radius) < _epsilon;
        }

        public int GetHashCode(BoundingSphere obj)
        {
            throw new NotImplementedException();
        }
    }

    public class Vector2Comparer : IEqualityComparer<Vector2>
    {
        static public Vector2Comparer Epsilon = new Vector2Comparer(0.000001f);

        private readonly float _epsilon;

        private Vector2Comparer(float epsilon)
        {
            _epsilon = epsilon;
        }

        public bool Equals(Vector2 x, Vector2 y)
        {
            return Math.Abs(x.X - y.X) < _epsilon &&
                   Math.Abs(x.Y - y.Y) < _epsilon;
        }

        public int GetHashCode(Vector2 obj)
        {
            throw new NotImplementedException();
        }
    }

    public class Vector3Comparer : IEqualityComparer<Vector3>
    {
        static public Vector3Comparer Epsilon = new Vector3Comparer(0.000001f);

        private readonly float _epsilon;

        private Vector3Comparer(float epsilon)
        {
            _epsilon = epsilon;
        }

        public bool Equals(Vector3 x, Vector3 y)
        {
            return Math.Abs(x.X - y.X) < _epsilon &&
                   Math.Abs(x.Y - y.Y) < _epsilon &&
                   Math.Abs(x.Z - y.Z) < _epsilon;
        }

        public int GetHashCode(Vector3 obj)
        {
            throw new NotImplementedException();
        }
    }

    public class Vector4Comparer : IEqualityComparer<Vector4>
    {
        static public Vector4Comparer Epsilon = new Vector4Comparer(0.000001f);

        private readonly float _epsilon;

        private Vector4Comparer(float epsilon)
        {
            _epsilon = epsilon;
        }

        public bool Equals(Vector4 x, Vector4 y)
        {
            return Math.Abs(x.X - y.X) < _epsilon &&
                   Math.Abs(x.Y - y.Y) < _epsilon &&
                   Math.Abs(x.Z - y.Z) < _epsilon &&
                   Math.Abs(x.W - y.W) < _epsilon;
        }

        public int GetHashCode(Vector4 obj)
        {
            throw new NotImplementedException();
        }
    }

    public class QuaternionComparer : IEqualityComparer<Quaternion>
    {
        static public QuaternionComparer Epsilon = new QuaternionComparer(0.000001f);

        private readonly float _epsilon;

        private QuaternionComparer(float epsilon)
        {
            _epsilon = epsilon;
        }

        public bool Equals(Quaternion x, Quaternion y)
        {
            return Math.Abs(x.X - y.X) < _epsilon &&
                   Math.Abs(x.Y - y.Y) < _epsilon &&
                   Math.Abs(x.Z - y.Z) < _epsilon &&
                   Math.Abs(x.W - y.W) < _epsilon;
        }

        public int GetHashCode(Quaternion obj)
        {
            throw new NotImplementedException();
        }
    }
    public class PlaneComparer : IEqualityComparer<Plane>
    {
        static public PlaneComparer Epsilon = new PlaneComparer(0.000001f);

        private readonly float _epsilon;

        private PlaneComparer(float epsilon)
        {
            _epsilon = epsilon;
        }

        public bool Equals(Plane x, Plane y)
        {
            return Math.Abs(x.Normal.X - y.Normal.X) < _epsilon &&
                   Math.Abs(x.Normal.Y - y.Normal.Y) < _epsilon &&
                   Math.Abs(x.Normal.Z - y.Normal.Z) < _epsilon &&
                   Math.Abs(x.D - y.D) < _epsilon;
        }

        public int GetHashCode(Plane obj)
        {
            throw new NotImplementedException();
        }
    }

    public static class ArrayUtil
    {
        public static T[] ConvertTo<T>(byte[] source) where T : struct
        {
            var sizeOfDest = Marshal.SizeOf(typeof(T));
            var count = source.Length / sizeOfDest;
            var dest = new T[count];

            var pinned = GCHandle.Alloc(source, GCHandleType.Pinned);
            var pointer = pinned.AddrOfPinnedObject();

            for (var i = 0; i < count; i++, pointer += sizeOfDest)
                dest[i] = (T)Marshal.PtrToStructure(pointer, typeof(T));

            pinned.Free();

            return dest;
        }

        public static byte[] ConvertFrom<T>(T[] source) where T : struct
        {
            var sizeOfSource = Marshal.SizeOf(typeof(T));
            var count = source.Length;
            var dest = new byte[sizeOfSource * count];

            var pinned = GCHandle.Alloc(dest, GCHandleType.Pinned);
            var pointer = pinned.AddrOfPinnedObject();

            for (var i = 0; i < count; i++, pointer += sizeOfSource)
                Marshal.StructureToPtr(source[i], pointer, true);

            pinned.Free();

            return dest;
        }
    }

    static class MathUtility
    {
        public static void MinMax(int a, int b, out int min, out int max)
        {
            if (a > b)
            {
                min = b;
                max = a;
            }
            else
            {
                min = a;
                max = b;
            }
        }
    }
}
