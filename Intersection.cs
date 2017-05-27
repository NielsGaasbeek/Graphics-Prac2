using OpenTK;

namespace Application
{
    class Intersection
    {
        //Vector3 intersectNorm;
        float intersectDist;
        Vector3 intersectPos;
        Primitive intersectObj;

        public Intersection(float t, Primitive P, Vector3 pos)
        {
            intersectDist = t;
            intersectObj = P;
            intersectPos = pos;
        }

        public float Distance
        {
            get { return intersectDist; }
        }

        public Primitive Primitive
        {
            get { return intersectObj; }
        }

        public Vector3 IntersectPosition
        {
            get { return intersectPos; }
        }
    }
}
