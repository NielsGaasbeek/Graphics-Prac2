using OpenTK;

namespace Application
{
    class Plane : Primitive
    {
        Vector3 normalVector;
        float distance;

        public Plane(string ID, Vector3 norm, float d, Vector3 color, string mat) : base(ID, color, new Vector3(0,0,0), mat)
        {
            normalVector = norm;
            distance = d;
        }

        public override float Intersection(Ray R)
        {
            float t = -(Vector3.Dot(R.O, normalVector) + distance) / (Vector3.Dot(R.D, normalVector));
            return t;
        }

        public override Vector3 NormalVector(Vector3 pos)
        {
            return normalVector;
        }
    }
}
