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

            normalVector = CrossProduct((v0 - v1), (v0 - v2)).Normalized();
        }

        public override float Intersection(Ray R)
        {
            float D = DotProduct(normalVector, v0);

            return ((DotProduct(normalVector, R.O) + D) / (DotProduct(normalVector, R.D)));
        }

        public override Vector3 NormalVector(Vector3 pos)
        {
            return normalVector;
        }

    }
}
