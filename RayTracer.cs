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

                    ray.D = new Vector3(u, v, 1) - renderCam.Position;
                    ray.O = renderCam.Position;
                    ray.Normalize();

                    Vector3 color = new Vector3(0, 0, 0);

                    Intersection I = scene.closestIntersect(ray);
                    if (I.Primitive != null)
                        color = Trace(ray, 0);

                    screen.Plot(
                        x, y,
                        CreateColor(
                            (int)color.X,
                            (int)color.Y,
                            (int)color.Z));

                    if (y == 256 && x % 10 == 0)
                        DrawDebugRay(ray, I);
                }
            }

            GL.End();
        }

        public Vector3 Trace(Ray ray, int recur)
        {
            Intersection I = scene.closestIntersect(ray);
            if (I.Primitive == null) return new Vector3(0, 0, 0);

            if (I.Primitive.PrimitiveMaterial == "Mirror")
            {
                if (recur < 16)
                    return I.Primitive.PrimitiveColor * Trace(Reflect(ray, I), recur++);
                return new Vector3(0, 0, 0);
            }
            else
                return DirectIllumination(I) * I.Primitive.PrimitiveColor;

        }
        
        public Ray Reflect(Ray ray, Intersection I)
        {
            Ray reflectRay = new Ray();
            Vector3 surfaceNormal = I.NormalVector;

            reflectRay.D = ray.D - ((2 * surfaceNormal) * (dotProduct(ray.D, surfaceNormal)));
            reflectRay.O = I.IntersectPosition;

            return reflectRay;
        }
        
        public Vector3 DirectIllumination(Intersection I)
        {
            Ray shadowRay = new Ray();
            Vector3 color = new Vector3(0, 0, 0);

            foreach (Light l in scene.Lights)
            {
                shadowRay.D = (I.IntersectPosition - l.Position);
                shadowRay.O = l.Position;
                float intersectDist = Length(shadowRay.D);
                shadowRay.Normalize();

                if (!IsVisible(I, shadowRay, intersectDist)) continue;

                float distAttenuation = l.Intensity / (intersectDist * intersectDist);
                float NdotL = dotProduct(I.NormalVector, shadowRay.D);
                if (NdotL < 0) continue;
                color += l.Color * distAttenuation * NdotL;
                continue;
            }

            return color;
        }

        public bool IsVisible(Intersection I, Ray L, float intersectDist)
        {
            foreach (Light l in scene.Lights)
            {
                Intersection lightIntersect = scene.closestIntersect(L);

                if ((int)(lightIntersect.Distance * 100) == (int)(intersectDist * 100))
                    return true;
                else continue;
            }
            return false;
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

            foreach (Sphere s in scene.Spheres)
            {
                for (float r = 0; r < 10; r += .1f)
                {
                    screen.Line(
                        TX((float)(s.PrimitivePosition.X + s.Radius * Math.Cos(r))) + 512,
                        TY((float)(s.PrimitivePosition.Z + s.Radius * Math.Sin(r))),
                        TX((float)(s.PrimitivePosition.X + s.Radius * Math.Cos(r + .1))) + 512,
                        TY((float)(s.PrimitivePosition.Z + s.Radius * Math.Sin(r + .1))),
                        CreateColor((int)s.PrimitiveColor.X, (int)s.PrimitiveColor.Y, (int)s.PrimitiveColor.Z)
                        );
                }
            }

        }

        public void DrawDebugRay(Ray ray, Intersection I)
        {
            //draws the primary ray in debug
            screen.Line(
                    TX(ray.O.X) + 512,
                    TY(ray.O.Z),
                    TX(ray.O.X + ray.D.X * I.Distance) + 512,
                    TY(ray.O.Z + ray.D.Z * I.Distance),
                    0xffff00
                        );

            //draws reflective rays
            if (I.Primitive.PrimitiveMaterial == "Mirror")
            {
                Ray reflectRay = Reflect(ray, I);
                I = scene.closestIntersect(reflectRay);

                screen.Line(
                    TX(reflectRay.O.X) + 512,
                    TY(reflectRay.O.Z),
                    TX(reflectRay.O.X + reflectRay.D.X * I.Distance) + 512,
                    TY(reflectRay.O.Z + reflectRay.D.Z * I.Distance),
                    0xffffff);
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

        float Length(Vector3 vec)
        {
            return (float)Math.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z));
        }

        public float dotProduct(Vector3 A, Vector3 B)
        {
            return ((A.X * B.X) + (A.Y * B.Y) + (A.Z * B.Z));
        }
    }
}