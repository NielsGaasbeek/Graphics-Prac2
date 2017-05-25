using OpenTK;

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

        public override float FindIntersection(Ray R)
        {
            float t = -(dotProduct(R.O, normalVector) + distance) / (dotProduct(R.D, normalVector));
            //if ((t < R.t) && (t > 0)) { R.t = t; }
            return t;
        }
    }
}
