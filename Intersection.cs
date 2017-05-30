using OpenTK;

namespace Application
{
    class Intersection
    {
        float intersectDist;
        Vector3 intersectPos;
        Primitive intersectObj;
        Vector3 intersectNormal;

        //constructor for an intersection. It stores the distance, primitive, position and the normal
        public Intersection(float t, Primitive P, Vector3 pos)
        {
            intersectDist = t;
            intersectObj = P;
            intersectPos = pos;
            if (P != null)
                intersectNormal = P.NormalVector(pos);
        }

        
        public Vector3 NormalVector
        { get { return intersectNormal; } }
        public float Distance
        { get { return intersectDist; } }
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
