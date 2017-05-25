using OpenTK;
using OpenTK.Graphics.OpenGL;
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

        public Camera renderCam;
        public Scene scene;
        public Sphere Sphere1, Sphere2, Sphere3;

        Ray ray = new Ray();

        //coordinate system
        float xmin = -5; float xmax = 5;
        float ymin = -5; float ymax = 5;
        float scale;

        float a;

        //initialize
        public void Init()
        {
            scale = (screen.height / (ymax - ymin));

            scene = new Scene(); //create the scene
            renderCam = new Camera(new Vector3(0, 0, -4), new Vector3(0, 0, 1)); //create the camera

            Light light = new Light(new Vector3(5, 5, 5), new Vector3(1, 1, 1)); //add a light to the scene
            scene.Lights.Add(light);

            Plane Floor = new Plane(new Vector3(0, 0, 0), 0, new Vector3(0.1f, 0.1f, 0.1f)); //gray floor plane
            Sphere1 = new Sphere(new Vector3(-3, 0, 3), 1, new Vector3(255, 0, 0)); //red sphere
            Sphere2 = new Sphere(new Vector3(0, 0, 3), 1, new Vector3(0, 255, 0)); //green sphere
            Sphere3 = new Sphere(new Vector3(3, 0, 3), 1, new Vector3(0, 0, 255)); //blue sphere

            scene.Primitives.Add(Floor); //add the primitives
            scene.Primitives.Add(Sphere1);
            scene.Primitives.Add(Sphere2);
            scene.Primitives.Add(Sphere3);
        }

        public void Render()
        {
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(PrimitiveType.Triangles);

            GL.End();
        }

        // tick: renders one frame
        public void Tick()
        {
            a += 0.01f;
            //screen.Clear(0);
            screen.Line(TX(5), TY(5), TX(5), TY(-5), 0xffffff);

            //debug view
            //camera
            screen.Plot(TX(renderCam.Position.X) + 512, TY(renderCam.Position.Z), 0xffffff); //x+512 voor rechterkant scherm
            screen.Plot(TX(renderCam.Position.X) + 513, TY(renderCam.Position.Z), 0xffffff);

            //screen plane
            screen.Line(TX(renderCam.p0.X) + 512, TY(renderCam.p0.Z), TX(renderCam.p1.X) + 512, TY(renderCam.p1.Z), 0xffffff);

            //spheres
            screen.Plot(TX((float)(Sphere1.CenterPos.X + Sphere1.Radius * Math.Cos(a))) + 512, TY((float)(Sphere1.CenterPos.Z + Sphere1.Radius * Math.Sin(a))), 
                CreateColor((int)Sphere1.Color.X, (int)Sphere1.Color.Y, (int)Sphere1.Color.Z));

            screen.Plot(TX((float)(Sphere2.CenterPos.X + Sphere2.Radius * Math.Cos(a))) + 512, TY((float)(Sphere2.CenterPos.Z + Sphere2.Radius * Math.Sin(a))),
                CreateColor((int)Sphere2.Color.X, (int)Sphere2.Color.Y, (int)Sphere2.Color.Z));

            screen.Plot(TX((float)(Sphere3.CenterPos.X + Sphere3.Radius * Math.Cos(a))) + 512, TY((float)(Sphere3.CenterPos.Z + Sphere3.Radius * Math.Sin(a))),
                CreateColor((int)Sphere3.Color.X, (int)Sphere3.Color.Y, (int)Sphere3.Color.Z));



        }

        public int TX(float x)
        {
            float tx = x + xmax;
            tx = scale * tx + ((screen.width / 4) + xmin * scale);

            return (int)tx;
        }

        public int TY(float y)
        {
            float ty = -y + ymax;
            ty *= scale;

            return (int)ty;
        }

        int CreateColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }
    }
}