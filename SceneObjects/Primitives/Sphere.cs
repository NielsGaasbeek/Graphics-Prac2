﻿using OpenTK;
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

        public override float Intersection(Ray R)
        {
            float a = Vector3.Dot(R.D, R.D);
            float b = Vector3.Dot(2 * R.D, (R.O - PrimitivePosition));
            float c = Vector3.Dot((R.O - PrimitivePosition), (R.O - PrimitivePosition)) - (radius * radius);

            float D = (float)Math.Sqrt((b*b)-(4*a*c));
            if(D >= 0)
            {
                Vector3 C = PrimitivePosition - R.O;
                float t = Vector3.Dot(C, R.D);
                Vector3 q = C - t * R.D;
                float p2 = Vector3.Dot(q, q);
                if (p2 > (radius * radius)) { return 0; }
                t -= (float)Math.Sqrt((radius * radius) - p2);
                if ((t < R.t) && (t > 0f)) { R.t = t; }
                return t;
            }
            else return 0;
        }

        public override Vector3 NormalVector(Vector3 pos)
        {
            return (PrimitivePosition - pos).Normalized();
        }

        public float Radius
        {
            get { return radius; }
        }
    }
}
