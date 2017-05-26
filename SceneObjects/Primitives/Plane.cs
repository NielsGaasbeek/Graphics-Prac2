using OpenTK;

namespace Application
{
    class Plane : Primitive
    {
        Vector3 normalVector;
        float distance;

        public Plane(Vector3 norm, float d, Vector3 color) : base(color, new Vector3(0,0,0))
        {
            normalVector = norm;
            distance = d;
        }

        public override float Intersection(Ray R)
        {
            float t = -(dotProduct(R.O, normalVector) + distance) / (dotProduct(R.D, normalVector));
            return t;
        }

        public override Vector3 NormalVector(Vector3 pos)
        {
            return normalVector;
        }
    }
}
