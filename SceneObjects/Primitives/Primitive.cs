﻿using OpenTK;
using System.Collections.Generic;

namespace Application
{
    class Primitive
    {
        Vector3 primitiveColor;

        public Primitive(Vector3 color)
        {
            primitiveColor = color;
        }

        public virtual float FindIntersection(Ray R)
        {
            return -1;
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
