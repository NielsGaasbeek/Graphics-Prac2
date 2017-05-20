using OpenTK;
using System;
using System.IO;

namespace Application
{
    struct Ray
    {
        Vector3 O; //origin
        Vector3 D; //Direction
        float t; //distance
    }

    class RayTracer
    {
        // member variables
        public Surface screen;

        public Camera renderCam, debugCam;
        public Scene scene;

        // initialize
        public void Init()
        {
            renderCam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, -1));
            debugCam = new Camera(new Vector3(5, 5, 10), new Vector3(0, -1, 0));

            scene = new Scene();
        }

        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);

        }
    }
}