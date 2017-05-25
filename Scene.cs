using OpenTK;
using System.Collections.Generic;

namespace Application
{
    class Scene
    {
        protected List<Light> lightList = new List<Light>();
        protected List<Primitive> sceneObjects = new List<Primitive>();
        protected List<Intersection> intersections = new List<Intersection>();

        public Scene()
        {

        }

        public Intersection Intersect(Ray R)
        {
            intersections.Clear(); //we only want the intersections of the current ray, so first we remove the ones from the previous ray

            foreach (Primitive P in sceneObjects) //loop over all primitives and find intersections
            {
                float t = P.FindIntersection(R);
                if(t > 0)
                {
                    intersections.Add(new Intersection(t, P));
                }
            }

            if(intersections.Count > 0)
            {
                Intersection closest = intersections[0]; //not sure about this line
                foreach(Intersection I in intersections) //loop over all intersections and see which one is closest
                {
                    if (I.Distance < closest.Distance)
                    {
                        R.t = I.Distance;
                        closest = I;
                    }
                }
                return closest;
            }

            return new Intersection(0, new Primitive(Vector3.Zero));
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
