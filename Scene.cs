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
            float tMax = int.MaxValue; //afstand tussen begin en eind ray is in het begin nog 0 tot oneindig
            Primitive hitObject = null; //we hebben nog niks geraakt

            foreach (Primitive P in sceneObjects) //voor elke primitive...
            {
                float t = P.Intersection(R); //kijken of er een intersection is
                if ( t > 0 && t < tMax) //als t>0 dan is er een intersection, als t < tMax is hij dichter bij de dan de vorige
                {
                    tMax = t;
                    hitObject = P;
                }
            }

            return new Intersection(tMax, hitObject); //return dichtstbijzijne intersection
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
