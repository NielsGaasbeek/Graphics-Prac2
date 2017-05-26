using OpenTK;
using System.Collections.Generic;

namespace Application
{
    class Scene
    {
        protected List<Light> lightList = new List<Light>();
        protected List<Primitive> sceneObjects = new List<Primitive>();

        public Scene()
        {

        }

        public Intersection closestIntersect(Ray R)
        {
            float tMax = int.MaxValue;
            Primitive hitObject = null;

            foreach (Primitive P in sceneObjects)
            {
                float t = P.Intersection(R);
                if ( t > 0 && t < tMax)
                {
                    tMax = t;
                    hitObject = P;
                }
            }

            return new Intersection(tMax, hitObject);
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
