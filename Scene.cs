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

        public Intersection Intersect(Ray R)
        {
            Intersection closest;

            foreach (Primitive P in sceneObjects)
            {
                P.FindIntersection(R);
            }

            return closest;
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
