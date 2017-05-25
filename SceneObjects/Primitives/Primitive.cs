using OpenTK;
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

        public virtual void FindIntersection(Ray R)
        {

        }

        public float dotProduct(Vector3 A, Vector3 B)
        {
            return A.X * B.X + A.Y * B.Y + A.Z * B.Z;
        }
    }
}
