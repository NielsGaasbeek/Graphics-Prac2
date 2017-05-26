using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Application
{
    class Primitive
    {
        Vector3 primitiveColor;
        private Vector3 position;

        public Primitive(Vector3 color, Vector3 pos)
        {
            primitiveColor = color;
            position = pos;
        }
        
        public virtual float Intersection(Ray R)
        {
            return 0;
        }

        public float dotProduct(Vector3 A, Vector3 B)
        {
            return A.X * B.X + A.Y * B.Y + A.Z * B.Z;
        }

        public Vector3 Position
        {
            get { return position; }
        }
        public Vector3 Color
        {
            get { return primitiveColor; }
        }
    }
}
