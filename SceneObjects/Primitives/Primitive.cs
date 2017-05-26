using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Application
{
    class Primitive
    {
        Vector3 primitiveColor;

        public Primitive(Vector3 color)
        {
            primitiveColor = color;
        }
        
        public virtual float Intersection(Ray R)
        {
            return 0;
        }

        public float dotProduct(Vector3 A, Vector3 B)
        {
            return A.X * B.X + A.Y * B.Y + A.Z * B.Z;
        }

        public Vector3 Color
        {
            get { return primitiveColor; }
        }
    }
}
