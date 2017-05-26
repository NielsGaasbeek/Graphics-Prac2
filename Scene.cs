using OpenTK;
using System.Collections.Generic;

namespace Application
{
    class Scene
    {
        protected List<Light> lightList;
        protected List<Primitive> sceneObjects;

        public Scene()
        {
            lightList = new List<Light>();
            sceneObjects = new List<Primitive>();
        }

        public Intersection closestIntersect(Ray R)
        {
            float tMin = int.MaxValue;
            Primitive hitObject = null;

            foreach (Primitive P in sceneObjects)
            {
                float t = P.Intersection(R);
                if ( t > 0 && t < tMin)
                {
                    tMin = t;
                    hitObject = P;
                }
            }

            return new Intersection(tMin, hitObject, (R.O + (tMin * R.D)));
        }

        public IList<Light> Lights
        {
            get { return lightList; }
        }

        public IList<Primitive> Primitives
        {
            get { return sceneObjects; }
        }
    }
}
