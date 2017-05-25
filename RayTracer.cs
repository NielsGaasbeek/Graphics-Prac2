using OpenTK;
using System;

namespace Application
{
    struct Ray
    {
        public Vector3 O; //origin
        public Vector3 D; //Direction
        public float t; //distance
    }

    class RayTracer
    {
        // member variables
        public Surface screen;

        public Camera renderCam, debugCam;
        public Scene scene;

        Ray ray = new Ray();

        //coordinate system
        float xmin = -5; float xmax = 5;
        float ymin = -5; float ymax = 5;
        float scale;

        //initialize
        public void Init()
        {
            scale = (screen.height / (ymax - ymin));

            renderCam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, -1)); //create the cameras, one for rendering, one for debug view
            debugCam = new Camera(new Vector3(5, 5, 10), new Vector3(0, -1, 0)); //not sure if this is how its supposed to be

            scene = new Scene(); //create the scene

            Light light = new Light(new Vector3(5, 5, 5), new Vector3(1, 1, 1)); //add a light to the scene
            scene.Lights.Add(light);

            Plane Floor = new Plane(new Vector3(0, 1, 0), 0, new Vector3(0.1f, 0.1f, 0.1f)); //gray floor plane
            Sphere Sphere1 = new Sphere(new Vector3(3, 3, 2), 2, new Vector3(1, 0, 0)); //red sphere
            //Sphere Sphere2 = new Sphere(new Vector3(0, 0, 0), 2, new Vector3(0, 1, 0)); //green sphere
            //Sphere Sphere3 = new Sphere(new Vector3(0, 0, 0), 2, new Vector3(0, 0, 1)); //blue sphere

            scene.Primitives.Add(Floor); //add the primitives
            scene.Primitives.Add(Sphere1);
            //scene.Primitives.Add(Sphere2);
            //scene.Primitives.Add(Sphere3);
        }

        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
            screen.Line(TX(5), TY(5), TX(5), TY(-5), 0xffffff);

            for (int x = 0; x < 512; x++)
            {
                for (int y = 0; y < screen.height; y++)
                {
                    //create ray
                    ray.O = renderCam.Position;
                    ray.D = new Vector3(TX(x), TY(y), 0) - renderCam.Position;

                    //find closest intersection and its surface normal n
                    scene.Intersect(ray);

                    //set pixel color computed from hit point, light and n
                }
            }
        }

        public int TX(float x)
        {
            float tx = x + xmax;
            tx = scale * tx + ((512 / 2) + xmin * scale);

            return (int)tx;
        }

        public int TY(float y)
        {
            float ty = -y + ymax;
            ty *= scale;

            return (int)ty;
        }
    }
}