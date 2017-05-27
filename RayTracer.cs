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

        public void Normalize()
        {
            float normFactor = (float)Math.Sqrt((D.X * D.X) + (D.Y * D.Y) + (D.Z * D.Z));
            D = new Vector3(D.X / normFactor, D.Y / normFactor, D.Z / normFactor);
        }
    }

    class RayTracer
    {
        // member variables
        public Surface screen;
        Surface floorTex;
        float[,] floorTexColors = new float[128, 128];

        public Camera renderCam;
        public Scene scene;

        Ray ray = new Ray();
        Vector3 point = new Vector3(0, 0, 0);

        //coordinate system
        float xmin = -5; float xmax = 5;
        float ymin = -1; float ymax = 9;
        float scale;

        //initialize
        public void Init()
        {
            scale = (screen.height / (ymax - ymin));

            scene = new Scene(); //create the scene
            renderCam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1)); //create the camera
            ray.O = renderCam.Position;

            floorTex = new Surface("../../assets/pattern.png"); //data for floor texture (only works with black/white for now)
            for (int x = 0; x < 128; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    float f = ((float)(floorTex.pixels[x + y * 128] & 255)) / 256;
                    if (f > 0) { f = 1; }
                    floorTexColors[x, y] = f;
                }
            }
        }

        public void Render()
        {
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(PrimitiveType.Triangles);

            for (int y = 0; y < 512; y++)
            {
                for (int x = 0; x < 512; x++)
                {
                    float u = (renderCam.p0.X + (renderCam.p1.X - renderCam.p0.X) * ((x + 0.5f) / 512));
                    float v = (renderCam.p0.Y + (renderCam.p2.Y - renderCam.p0.Y) * ((y + 0.5f) / 512));
<<<<<<< HEAD
                    float w = (renderCam.p0.Z + (renderCam.p2.Z - renderCam.p0.Z) * ((y + 0.5f) / 512)); //deze regel was er eerst niet

                    Vector3 dir = new Vector3(u, v, w) - renderCam.Position; //die w was eerst 1
=======

                    Vector3 dir = new Vector3(u, v, 1) - renderCam.Position;
>>>>>>> refs/remotes/origin/master
                    float normal = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y) + (dir.Z * dir.Z));
                    Vector3 normDir = new Vector3(dir.X / normal, dir.Y / normal, dir.Z / normal);

                    ray.D = normDir;
                    ray.O = renderCam.Position;

                    Intersection intersection = scene.closestIntersect(ray);

                    if (intersection.Primitive != null)
                    {
                        //here the shadowrays should be fired
                        Ray shadowRay = new Ray();
                        foreach (Light l in scene.Lights)
                        {
                            shadowRay.D = (intersection.IntersectPosition - l.Position);
                            shadowRay.O = l.Position;
                            shadowRay.Normalize();
                            float intersectDist = CalcDistance(shadowRay.O, intersection.IntersectPosition);

                            Intersection lightIntersect = scene.closestIntersect(shadowRay);

                            if ((int)(lightIntersect.Distance) == (int)intersectDist)
                            {
                                float distAttenuation = l.Intensity / (intersectDist * intersectDist);
                                float NdotL = dotProduct(intersection.Primitive.NormalVector(intersection.IntersectPosition), shadowRay.D);
                                if (NdotL < 0 || distAttenuation < .05f) continue;

                                Vector3 color = intersection.Primitive.Color * distAttenuation * NdotL;

                                if (intersection.Primitive is Plane) //de enigste plane is de vloer. als er meer planes zijn moet het anders of elke plane krijgt dezelfde texture
                                {
                                    color = shadePoint(intersection.IntersectPosition, floorTex) * distAttenuation * NdotL;
                                }

                                screen.Plot(x, y, CreateColor((int)color.X, (int)color.Y, (int)color.Z));
                            }
                        }
                    }

                    if (y == 256 && x % 30 == 0)
                    {
                        screen.Line(TX(ray.O.X) + 512, TY(ray.O.Z), TX(ray.O.X + ray.D.X * intersection.Distance) + 512, TY(ray.O.Z + ray.D.Z * intersection.Distance), 0xffff00);
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
            screen.Line(TX(renderCam.p0.X) + 512, TY(renderCam.p0.Z), TX(renderCam.p1.X) + 512, TY(renderCam.p1.Z), 0xffffff);

            foreach (Sphere s in scene.Spheres)
            {
                for (float r = 0; r < 10; r += .1f)
                {
                    screen.Line(TX((float)(s.Position.X + s.Radius * Math.Cos(r))) + 512, TY((float)(s.Position.Z + s.Radius * Math.Sin(r))),
                        TX((float)(s.Position.X + s.Radius * Math.Cos(r + .1))) + 512, TY((float)(s.Position.Z + s.Radius * Math.Sin(r + .1))),
                        CreateColor((int)s.Color.X, (int)s.Color.Y, (int)s.Color.Z));
                }
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
            if (red > 255) red = 255;
            if (green > 255) green = 255;
            if (blue > 255) blue = 255;
            return (red << 16) + (green << 8) + blue;
        }

        float CalcDistance(Vector3 pos1, Vector3 pos2)
        {
            return (float)Math.Sqrt((pos1.X - pos2.X) * (pos1.X - pos2.X) + (pos1.Y - pos2.Y) * (pos1.Y - pos2.Y) + (pos1.Z - pos2.Z) * (pos1.Z - pos2.Z));
        }

        public float dotProduct(Vector3 A, Vector3 B)
        {
            return A.X * B.X + A.Y * B.Y + A.Z * B.Z;
        }

        public Vector3 shadePoint(Vector3 P, Surface T) //volgens het boek, maar ik geloof niet dat dit helemaal nodig is
        {
            Vector2 coord = new Vector2(P.X, P.Z);
            float f = textureLookup(T, coord.X, coord.Y);
            return new Vector3((int)f * 255, (int)f * 255, (int)f * 255);
        }

        public float textureLookup(Surface T, float u, float v)
        {
            //plane is veel groter dan de texture, dus als de positie buiten de texture (0-128) valt, coordinaten opschuiven
            int i = (int)Math.Round(u * T.width - 0.5); 
            while (i < 0)
            {
                i += 128;
            }
            while (i >= 128)
            {
                i -= 128;
            }
            int j = (int)Math.Round(v * T.height - 0.5);
            while (j < 0)
            {
                j += 128;
            }
            while (j >= 128)
            {
                j -= 128;
            }
            return floorTexColors[i, j];
        }
    }
}