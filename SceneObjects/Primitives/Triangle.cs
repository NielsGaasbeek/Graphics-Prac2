using System;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    class Triangle : Primitive
    {
        Vector3 v0, v1, v2;
        Vector3 normalVector;

        public Triangle(string ID, Vector3 vert0, Vector3 vert1, Vector3 vert2, Vector3 color, string mat) : base(ID, color, vert0, mat)
        {
            v0 = vert0;
            v1 = vert1;
            v2 = vert2;

            normalVector = CrossProduct((v1 - v0), (v2 - v0)).Normalized();
        }

        public override float Intersection(Ray R)
        {
            Vector3 e1, e2;
            Vector3 P, Q, T;
            float det, inv_det, u, v;
            float t;

            e1 = v1 - v0;
            e2 = v2 - v0;

            P = CrossProduct(R.D, e2);
            det = DotProduct(e1, P);
            if (det > -0.0001f && det < 0.0001f) return 0f;
            inv_det = 1f / det;

            T = R.O - v0;

            u = DotProduct(T, P) * inv_det;

            if (u < 0f || u > 1f) return 0f;

            Q = CrossProduct(T, e1);

            v = DotProduct(R.D, Q) * inv_det;
            if (v < 0f || u + v > 1f) return 0f;

            t = DotProduct(e2, Q) * inv_det;
            if (t > .0001f)
                return t;
            return 0f;
        }

        public override Vector3 NormalVector(Vector3 pos)
        {
            return normalVector;
        }

    }
}
