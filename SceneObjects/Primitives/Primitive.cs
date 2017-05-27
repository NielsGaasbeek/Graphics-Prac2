using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Application
{
    class Primitive
    {
        Vector3 primitiveColor;
        private Vector3 position;
        string material;


        public Primitive(Vector3 color, Vector3 pos, string mat)
        {
            primitiveColor = color;
            position = pos;
            material = mat;
        }
        
        public virtual float Intersection(Ray R)
        {
            return 0;
        }

        public virtual Vector3 NormalVector(Vector3 pos)
        {
            return new Vector3(0, 0, 0);
        }

        public float dotProduct(Vector3 A, Vector3 B)
        {
            return A.X * B.X + A.Y * B.Y + A.Z * B.Z;
        }


        public Vector3 PrimitivePosition
        { get { return position; } }
        public Vector3 PrimitiveColor
        { get { return primitiveColor; } }
        public string PrimitiveMaterial
        { get { return material; } }
    }
}
