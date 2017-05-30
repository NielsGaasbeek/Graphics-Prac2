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

            FillScene();
        }

        //method to find the closest intersection in the scene
        public Intersection closestIntersect(Ray R)
        {
            float tMin = int.MaxValue; //start with the ray distance at (imaginary) infinite
            Primitive hitObject = null; //nothing has been hit yet

            foreach (Primitive P in sceneObjects) //check for each primitive
            {
                float t = P.Intersection(R); //the distance of the intersection
                if ( t > 0 && t < tMin) //if there is an interssection and it's distance is smaller than the privious smallest
                {
                    tMin = t; //set the closest to the one we just found
                    hitObject = P; //set the primitive to the one we just found
                }
            }
            return new Intersection(tMin, hitObject, (R.O + (tMin * R.D))); //return the closest intersection
        }

        public IList<Light> Lights
        {
            get { return lightList; }
        }

        public IList<Primitive> Primitives
        {
            get { return sceneObjects; }
        }

        Sphere Sphere1, Sphere2, Sphere3;
        Plane Floor;

        //method to fill the scene
        public void FillScene()
        {
            //add lights to the scene
            Light light1 = new Light(new Vector3(-2, -2, 1), new Vector3(1, 0, 0), 10); 
            Light light2 = new Light(new Vector3(2, -2, 7), new Vector3(0, 1, 0), 10);
            Light bigLight = new Light(new Vector3(0, -3, 2), new Vector3(1, 1, 1), 25);

            Lights.Add(bigLight);
            Lights.Add(light1);
            Lights.Add(light2);

            //add primitives to the scene
            Floor = new Plane( "Floor", new Vector3(0, 1, 0), -1, new Vector3(50, 50, 50), "Specular"); //gray floor plane
            Sphere1 = new Sphere("Sphere1", new Vector3(-2, 0, 4), 1, new Vector3(255, 0, 0), "Specular"); //left sphere
            Sphere2 = new Sphere("Sphere2", new Vector3(0, 0, 5), 1, new Vector3(255, 255, 255), "Mirror"); //middle sphere

            Sphere3 = new Sphere("Sphere3", new Vector3(3, 0, 5), 1, new Vector3(0, 0, 255), "Specular"); //right sphere

            Triangle Triangle1 = new Triangle("Triangle1", new Vector3(0,1,3), new Vector3(1,-2,4), new Vector3(2,1,3), new Vector3(0,0,255),"Diffuse");


            sceneObjects.Add(Floor); //add the primitives

            sceneObjects.Add(Sphere1);

            sceneObjects.Add(Sphere2);

            sceneObjects.Add(Sphere3);

            sceneObjects.Add(Triangle1);
        }
    }
}
