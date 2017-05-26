using OpenTK;
using System;

namespace Application
{
    class Sphere : Primitive
    {
        float radius;

        public Sphere(Vector3 pos, float r, Vector3 color) : base(color, pos)
        {
            radius = r;
        }

        public override float Intersection(Ray R)
        {
            float a = dotProduct(R.D, R.D);
            float b = dotProduct(2 * R.D, (R.O - Position));
            float c = dotProduct((R.O - Position), (R.O - Position)) - (radius * radius);

            float D = (float)Math.Sqrt((b*b)-(4*a*c));
            if(D >= 0)
            {
                Vector3 C = Position - R.O;
                float t = dotProduct(C, R.D);
                Vector3 q = C - t * R.D;
                float p2 = dotProduct(q, q);
                if (p2 > (radius * radius)) { return 0; }
                t -= (float)Math.Sqrt((radius * radius) - p2);
                if ((t < R.t) && (t > 0f)) { R.t = t; }
                return t;
            }
            else return 0;
        }

        public float Radius
        {
            get { return radius; }
        }
    }
}
