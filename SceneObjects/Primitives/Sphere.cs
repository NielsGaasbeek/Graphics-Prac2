using OpenTK;
using System;

namespace Application
{
    class Sphere : Primitive
    {
        float radius;

        public Sphere(string ID, Vector3 pos, float r, Vector3 color, string mat) : base(ID, color, pos, mat)
        {
            radius = r;
        }

        //method to find an intersection
        public override float Intersection(Ray R)
        {
            //Jacco's intersection calculation from the ray tracing presentation
            Vector3 C = PrimitivePosition - R.O; 
            float t = Vector3.Dot(C, R.D);
            Vector3 q = C - t * R.D;
            float p2 = Vector3.Dot(q, q);
            if (p2 > (radius * radius)) { return 0; } //if p^2 is greater than the sphere's radius^2, we are sure that we miss the sphere and we stop
            t -= (float)Math.Sqrt((radius * radius) - p2); //otherwise we continue calculation and return the distance t of the calculation
            if ((t < R.t) && (t > 0f)) { R.t = t; }
            return t;
        }

        //returns the normal at a given position on the sphere
        public override Vector3 NormalVector(Vector3 pos)
        {
            return (PrimitivePosition - pos).Normalized();
        }

        //returns the radius
        public float Radius
        {
            get { return radius; }
        }
    }
}
