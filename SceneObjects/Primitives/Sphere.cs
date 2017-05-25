using OpenTK;
using System;

namespace Application
{
    class Sphere : Primitive
    {
        Vector3 centerPos;
        float radius;

        public Sphere(Vector3 pos, float r, Vector3 color) : base(color)
        {
            centerPos = pos;
            radius = r;
        }

        public override float FindIntersection(Ray R)
        {
            Vector3 c = this.centerPos - R.O;
            float t = dotProduct(c, R.D);
            Vector3 q = c - t * R.D;
            float pSquared = dotProduct(q, q);
            if (pSquared > this.radius * this.radius) { return -1; }
            t -= (float)Math.Sqrt(this.radius * this.radius - pSquared);
            //if ((t < R.t) && (t > 0)) { R.t = t; }
            return t;
        }

        public Vector3 CenterPos
        {
            get { return centerPos; }
        }

        public float Radius
        {
            get { return radius; }
        }
    }
}
