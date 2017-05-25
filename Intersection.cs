using OpenTK;

namespace Application
{
    class Intersection
    {
        //Vector3 intersectNorm;
        float intersectDist;
        Primitive intersectObj;

        public Intersection(float t, Primitive P)
        {
            intersectDist = t;
            intersectObj = P;
        }

        public float Distance
        {
            get { return intersectDist; }
        }

        public Primitive Primitive
        {
            get { return intersectObj; }
        }
    }
}
