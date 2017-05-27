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
            Light light1 = new Light(new Vector3(-4, -1, 4), new Vector3(0, 0, 1), 10); //add a light to the scene
            Light light2 = new Light(new Vector3(2, -2, 4), new Vector3(1, 0, 0), 10);
            Light bigLight = new Light(new Vector3(0, -3, 2), new Vector3(1, 1, 1), 15);
            //Lights.Add(light2);
            //Lights.Add(light1);
            Lights.Add(bigLight);


            Floor = new Plane( "Floor", new Vector3(0, 1, 0), -1, new Vector3(50, 50, 50), "Specular"); //gray floor plane
            Sphere1 = new Sphere("Sphere1", new Vector3(-2, 0, 4), 1, new Vector3(255, 0, 0), "Specular"); //left sphere
            Sphere2 = new Sphere("Sphere2", new Vector3(0, 0, 5), 1, new Vector3(1, 1, 1), "Mirror"); //middle sphere
            Sphere3 = new Sphere("Sphere3", new Vector3(3, 0, 5), 1, new Vector3(0, 0, 255), "Specular"); //right sphere


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
