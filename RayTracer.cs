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

        public void Normalize(Ray r)
        {
            float normFactor = (float)Math.Sqrt((D.X * D.X) + (D.Y * D.Y) + (D.Z * D.Z));
            D = new Vector3(r.D.X / normFactor, r.D.Y / normFactor, r.D.Z / normFactor);
        }
    }

    class RayTracer
    {
        // member variables
        public Surface screen;

        public Camera renderCam;
        public Scene scene;
        public Sphere Sphere1, Sphere2, Sphere3;
        Plane Floor;

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
            
            Light light = new Light(new Vector3(5, 5, 5), new Vector3(1, 1, 1), 10); //add a light to the scene
            scene.Lights.Add(light);

            Floor = new Plane(new Vector3(0, 1, 0), 0, new Vector3(100, 100, 100)); //gray floor plane
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

            for(int y = 0; y < 512; y++)
            {
                for(int x = 0; x < 512; x++)
                {
                    float u = (renderCam.p0.X + (renderCam.p1.X - renderCam.p0.X) * ((x + 0.5f) / 512));
                    float v = (renderCam.p0.Y + (renderCam.p2.Y - renderCam.p0.Y) * ((y + 0.5f) / 512));
                    
                    Vector3 dir = new Vector3(u, v, 1) - renderCam.Position;
                    float normal = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y) + (dir.Z * dir.Z));
                    Vector3 normDir = new Vector3(dir.X / normal, dir.Y / normal, dir.Z / normal);

                    ray.D = normDir;
                    ray.O = renderCam.Position;

                    Intersection intersection = scene.closestIntersect(ray);

                    if(intersection.Primitive != null)
                    {
                        //here the shadowrays should be fired

                        screen.Plot(
                            x, y, 
                            CreateColor(
                                (int)intersection.Primitive.Color.X, 
                                (int)intersection.Primitive.Color.Y, 
                                (int)intersection.Primitive.Color.Z
                                        )
                                    );
                    }

                    if(y == 256 && x % 30 == 0)
                    {
                        screen.Line(
                            TX(ray.O.X) + 512, 
                            TY(ray.O.Z), 
                            TX(ray.O.X + ray.D.X * intersection.Distance) + 512, 
                            TY(ray.O.Z + ray.D.Z * intersection.Distance), 
                            0xffff00);
                    }                    
                }
            }

            GL.End();
        }

        // tick: renders one frame
        public void Tick()
        {
            //screen.Clear(0);
            screen.Line(TX(5), TY(ymax), TX(5), TY(ymin), 0xffffff);

            //debug view
            //camera
            screen.Plot(TX(renderCam.Position.X) + 512, TY(renderCam.Position.Z), 0xffffff); //x+512 voor rechterkant scherm
            screen.Plot(TX(renderCam.Position.X) + 513, TY(renderCam.Position.Z), 0xffffff);

            //screen plane
            screen.Line(
                TX(renderCam.p0.X) + 512, 
                TY(renderCam.p0.Z), 
                TX(renderCam.p1.X) + 512, 
                TY(renderCam.p1.Z), 0xffffff);

            //spheres
            for (int r = 0; r < 360; r++)
            {
                screen.Plot(
                    TX((float)(Sphere1.CenterPos.X + Sphere1.Radius * Math.Cos(r))) + 512, 
                    TY((float)(Sphere1.CenterPos.Z + Sphere1.Radius * Math.Sin(r))), 
                    CreateColor((int)Sphere1.Color.X, (int)Sphere1.Color.Y, (int)Sphere1.Color.Z));

                screen.Plot(
                    TX((float)(Sphere2.CenterPos.X + Sphere2.Radius * Math.Cos(r))) + 512, 
                    TY((float)(Sphere2.CenterPos.Z + Sphere2.Radius * Math.Sin(r))),
                    CreateColor((int)Sphere2.Color.X, (int)Sphere2.Color.Y, (int)Sphere2.Color.Z));

                screen.Plot(
                    TX((float)(Sphere3.CenterPos.X + Sphere3.Radius * Math.Cos(r))) + 512, 
                    TY((float)(Sphere3.CenterPos.Z + Sphere3.Radius * Math.Sin(r))),
                    CreateColor((int)Sphere3.Color.X, (int)Sphere3.Color.Y, (int)Sphere3.Color.Z));
            }


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