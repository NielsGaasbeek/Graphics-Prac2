using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;

namespace Application
{
    class Plane : Primitive
    {
        Vector3 normalVector;
        float distance;

        public Plane(Vector3 norm, float d, Vector3 color) : base(color)
        {
            normalVector = norm;
            distance = d;
        }

    }
}
