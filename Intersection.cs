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
            set { intersectDist = value; }
        }

        public Primitive Primitive
        {
            get { return intersectObj; }
            set { intersectObj = value; }
        }
    }
}
