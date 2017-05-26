using OpenTK;
using System.Collections.Generic;

namespace Application
{
    class Scene
    {
        protected List<Light> lightList;
        protected List<Primitive> sceneObjects;
        IList<Sphere> spheres;

        public Scene()
        {
            lightList = new List<Light>();
            sceneObjects = new List<Primitive>();
            spheres = new List<Sphere>();

            FillScene();
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

        public IList<Sphere> Spheres
        {
            get { return spheres; }
        }


        Sphere Sphere1, Sphere2, Sphere3;
        Plane Floor;

        public void FillScene()
        {
            Light light = new Light(new Vector3(5, 5, 5), new Vector3(1, 1, 1), 10); //add a light to the scene
            Lights.Add(light);

            Floor = new Plane(new Vector3(0, 1, 0), -1, new Vector3(100, 100, 100)); //gray floor plane
            Sphere1 = new Sphere(new Vector3(-3, 0, 7), 1, new Vector3(255, 100, 0)); //red sphere
            Sphere2 = new Sphere(new Vector3(0, 0, 7), 1, new Vector3(0, 255, 0)); //green sphere
            Sphere3 = new Sphere(new Vector3(3, 0, 7), 1, new Vector3(0, 0, 255)); //blue sphere

            sceneObjects.Add(Floor); //add the primitives

            sceneObjects.Add(Sphere1);
            spheres.Add(Sphere1);

            sceneObjects.Add(Sphere2);
            spheres.Add(Sphere2);

            sceneObjects.Add(Sphere3);
            spheres.Add(Sphere3);
        }
    }
}
