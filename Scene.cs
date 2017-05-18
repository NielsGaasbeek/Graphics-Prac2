using System;
using System.Collections.Generic;
using OpenTK;

namespace Application
{
    class Scene
    {
        IList<Light> lightList;
        IList<Primitive> sceneObjects;

        public Scene()
        {
            Plane Floor = new Plane(new Vector3(0, 1, 0), 0, new Vector3(0.1f, 0.1f, 0.1f)); //grijze vloer plane
            Sphere Sphere1 = new Sphere(new Vector3(-1, 0, 5), 2, new Vector3(1, 0, 0)); //rode bol
            Sphere Sphere2 = new Sphere(new Vector3(0, 0, 5), 2, new Vector3(0, 1, 0)); //groene bol
            Sphere Sphere3 = new Sphere(new Vector3(1, 0, 5), 2, new Vector3(0, 0, 1)); //blauwe bol

            //zet ze in de lijst
            sceneObjects.Add(Floor);
            sceneObjects.Add(Sphere1);
            sceneObjects.Add(Sphere2);
            sceneObjects.Add(Sphere3);
        }

        public void Intersect()
        {

        }
    }
}
