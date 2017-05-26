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

        public override float Intersection(Ray R)
        {
            float a = dotProduct(R.D, R.D);
            float b = dotProduct(2 * R.D, (R.O - centerPos));
            float c = dotProduct((R.O - centerPos), (R.O - centerPos)) - radius * radius;

            float D = (float)Math.Sqrt((b*b)-4*a*c); //abc-formule, als Discriminant >= 0 is er intersection
            if(D >= 0)
            {
                Vector3 C = centerPos - R.O;
                float t = dotProduct(C, R.D);
                Vector3 q = C - t * R.D;
                float p2 = dotProduct(q, q);
                if (p2 > (radius * radius))
                {
                    return 0;
                }
                t -= (float)Math.Sqrt((radius * radius) - p2);
                return t;
            }

            return 0;
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
