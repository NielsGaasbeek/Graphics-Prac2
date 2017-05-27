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
        Material material;
        string ID;


        public Primitive(string ID, Vector3 color, Vector3 pos, string mat)
        {
            this.ID = ID;
            primitiveColor = color;
            position = pos;
            material = new Material(mat);
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

        public string PrimitiveID
        { get { return ID; } }
        public Vector3 PrimitivePosition
        { get { return position; } }
        public Vector3 PrimitiveColor
        { get { return primitiveColor; } }
        public Material PrimitiveMaterial
        { get { return material; } }
    }
}
