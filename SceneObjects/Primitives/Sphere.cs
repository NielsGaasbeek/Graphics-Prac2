using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;

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

    }
}
