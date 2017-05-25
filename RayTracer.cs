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
        Vector3 point = new Vector3(0, 0, 0);

        //coordinate system
        float xmin = -5; float xmax = 5;
        float ymin = -1; float ymax = 9;
        float scale;

        float a;

        //initialize
        public void Init()
        {
            scale = (screen.height / (ymax - ymin));

            scene = new Scene(); //create the scene
            renderCam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1)); //create the camera
            ray.O = renderCam.Position;
            //ray.t = 10;

            Light light = new Light(new Vector3(5, 5, 5), new Vector3(1, 1, 1)); //add a light to the scene
            scene.Lights.Add(light);

            Plane Floor = new Plane(new Vector3(0, 0, 0), 0, new Vector3(0.1f, 0.1f, 0.1f)); //gray floor plane
            Sphere1 = new Sphere(new Vector3(-3, 0, 7), 1, new Vector3(255, 0, 0)); //red sphere
            Sphere2 = new Sphere(new Vector3(0, 0, 7), 1, new Vector3(0, 255, 0)); //green sphere
            Sphere3 = new Sphere(new Vector3(3, 0, 7), 1, new Vector3(0, 0, 255)); //blue sphere

            scene.Primitives.Add(Floor); //add the primitives
            scene.Primitives.Add(Sphere1);
            scene.Primitives.Add(Sphere2);
            scene.Primitives.Add(Sphere3);
        }

        public void Render()
        {
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(PrimitiveType.Triangles);

            for(int x = 0; x < 512; x++)
            {
                for(int y = 0; y < 512; y++)
                {
                    float u = (float)(renderCam.p0.X + (renderCam.p1.X - renderCam.p0.X) * (x + 0.5)) / 512;
                    float v = (float)(renderCam.p2.Y + (renderCam.p0.Y - renderCam.p2.Y) * (y + 0.5)) / 512;

                    Vector3 point = renderCam.p0 + u * (renderCam.p1 - renderCam.p0) + v * (renderCam.p2 - renderCam.p0);
                    Vector3 dir = point - renderCam.Position;
                    float normal = (float)Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y + dir.Z * dir.Z);
                    ray.D = new Vector3(dir.X / normal, dir.Y / normal, dir.Z / normal);

                    scene.Intersect(ray);

                    if(ray.t == 0)
                    {
                        screen.Plot(x, y, 0x000000);
                    }
                    else
                    {
                        screen.Plot(x, y, 0xffffff);
                    }

                    if(x % 20 == 0 && y % 20 == 0)
                    {
                        screen.Line(TX(ray.O.X) + 512, TY(ray.O.Z), TX(ray.O.X + ray.D.X * ray.t) + 512, TY(ray.O.Z + ray.D.Z * ray.t), 0xffff00);
                    }

                }
            }
            GL.End();
        }

        // tick: renders one frame
        public void Tick()
        {
            a += 0.01f;
            //screen.Clear(0);
            screen.Line(TX(5), TY(ymax), TX(5), TY(ymin), 0xffffff);

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

            //rays


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