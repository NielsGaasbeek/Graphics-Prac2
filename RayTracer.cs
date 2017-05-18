using OpenTK;
using System;
using System.IO;

namespace Application
{
    class RayTracer
    {
        // member variables
        public Surface screen;

        public Camera renderCam, debugCam;
        public Scene scene;

        // initialize
        public void Init()
        {
            renderCam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, -1), screen);
            debugCam = new Camera(new Vector3(5, 5, 10), new Vector3(0, -1, 0), screen);

            scene = new Scene();
        }

        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffffff);
            screen.Line(2, 20, 160, 20, 0xff0000);
        }

        //maak een ray
        public void Ray()
        {

        }
    }
}