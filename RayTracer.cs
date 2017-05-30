using OpenTK;
using System;
using System.Drawing;

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
        Surface environment; //HDR picture
        float[,] floorTexColors = new float[128, 128];

        int width = 512, height = 512;
        Vector3[] image;
        float[] AA = new float[4*2];

        public Camera renderCam;
        public Scene scene;

        Ray ray = new Ray();

        //coordinate system
        float xmin = -5; float xmax = 5;
        float ymin = -1; float ymax = 9;
        float scale;

        //initialize
        public void Init()
        {
            scale = (screen.height / (ymax - ymin));
            image = new Vector3[(width * height)];

            scene = new Scene(); //create the scene
            renderCam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 90); //create the camera. the last argument is the FOV in degrees
            ray.O = renderCam.position;

            //offset-array used for Anti-Aliasing
            AA[0] = -1f / 4f; AA[1] = -1f / 4f;
            AA[2] = -1f / 4f; AA[3] = 1f / 4f;
            AA[4] = 1f / 4f; AA[5] = -1f / 4f;
            AA[6] = 1f / 4f; AA[7] = 1f / 4f;

            environment = new Surface("../../assets/uffizi_probe.png");
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
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 color = new Vector3(0, 0, 0);

                    Intersection I = null;
                    for (int sample = 0; sample < 4; sample++)
                    {                        
                        float normalized_x = (x + 0.5f + AA[2 * sample]) / width ;
                        float normalized_y = (y + 0.5f + AA[2 * sample + 1]) / height;

                        Vector3 imagePoint = renderCam.p0 + 
                                            (normalized_x * renderCam.right_direction * 2) - 
                                            (normalized_y * renderCam.up_direction * 2);
                        Vector3 dir = imagePoint - renderCam.position;
                        ray.D = dir;

                        ray.O = renderCam.position;
                        ray.Normalize();


                        I = scene.closestIntersect(ray);
                        if (I.Primitive != null)
                            color += Trace(ray, 0);
                        else
                        {
                            color += GetEnvironment(ray);
                        }
                    }
                    color /= 4.0f;

                    image[x + width * y] = color;


                    if (y == 256 && x % 30 == 0)
                        DrawDebugRay(ray, I, 0xffff00);
                }
            }
            RenderScreen();
        }

        void RenderScreen()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 color = image[x + width * y];

                    screen.Plot(
                            x, y,
                            CreateColor(
                                (int)color.X,
                                (int)color.Y,
                                (int)color.Z));
                }
            }
        }

        public Vector3 Trace(Ray ray, int recur)
        {
            Intersection I = scene.closestIntersect(ray);
            if (I.Primitive == null) return GetEnvironment(ray);

            Vector3 primColor = I.Primitive.PrimitiveColor;


            if (I.Primitive.PrimitiveMaterial.isMirror)
            {
                if (recur < 1)
                    return primColor * Trace(Reflect(ray, I), recur++);
                return new Vector3(1, 1, 1);
            }
            else if (I.Primitive.PrimitiveMaterial.isDiElectric)
            {

            }
            else if (I.Primitive.PrimitiveMaterial.isSpecular)
            {
                Vector3 shadingCol = DirectIllumination(I);
                Vector3 reflectCol = Trace(Reflect(ray, I), recur++);

                reflectCol /= 255;

                shadingCol *= .5f;
                reflectCol *= .5f;

                if (I.Primitive is Plane) //de enige plane is de vloer. als er meer planes zijn moet het anders of elke plane krijgt dezelfde texture
                    primColor = shadePoint(I.IntersectPosition, floorTex);

                return primColor * (shadingCol + reflectCol);
            }

            if (I.Primitive is Plane) //de enige plane is de vloer. als er meer planes zijn moet het anders of elke plane krijgt dezelfde texture
                primColor = shadePoint(I.IntersectPosition, floorTex);

            return DirectIllumination(I) * primColor;

        }

        public Ray Reflect(Ray ray, Intersection I)
        {
            Ray reflectRay = new Ray();
            Vector3 surfaceNormal = I.NormalVector;

            reflectRay.D = ray.D - ((2 * surfaceNormal) * (Vector3.Dot(ray.D, surfaceNormal)));
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
                float NdotL = Vector3.Dot(I.NormalVector, shadowRay.D);
                if (NdotL < 0) continue;
                color += l.Color * distAttenuation * NdotL;
                continue;
            }

            return color;
        }

        public Vector3 GetEnvironment(Ray ray)
        {
            //bereken HDR coords
            float r = (float)((1 / Math.PI) * Math.Acos(ray.D.Z) / Math.Sqrt(ray.D.X * ray.D.X + ray.D.Y * ray.D.Y));
            float HDRx = MathHelper.Clamp(((ray.D.X * r + 1) * environment.width/2), 0, environment.width - 1);
            float HDRy = MathHelper.Clamp(((ray.D.Y * r + 1) * environment.height/2), 0, environment.height - 1);
            Color pixelCol = environment.bmp.GetPixel((int)HDRx, (int)HDRy);
            return new Vector3(pixelCol.R, pixelCol.G, pixelCol.B);
        }

        public bool IsVisible(Intersection I, Ray L, float intersectDist)
        {
            Intersection lightIntersect = scene.closestIntersect(L);

            if ((int)(lightIntersect.Distance * 10) == (int)(intersectDist * 10))
                return true;

            return false;
        }

        // tick: renders one frame
        public void DebugOutput()
        {
            //screen.Clear(1);
            screen.Line(TX(5), TY(ymax), TX(5), TY(ymin), 0xffffff);

            //debug view
            //camera

            screen.Plot(TX(renderCam.position.X) + 512, TY(renderCam.position.Z), 0xffffff); //x+512 voor rechterkant scherm
            screen.Plot(TX(renderCam.position.X) + 513, TY(renderCam.position.Z), 0xffffff);
            screen.Print("Camera: (" + Math.Round(renderCam.position.X, 1) + "; " + Math.Round(renderCam.position.Y, 1) + "; " + Math.Round(renderCam.position.Z, 1) + ")", 513, 25, 0xffffff);
            screen.Print("Camera-Downward Angle: " + renderCam.camera_direction.Y, 720, 5, 0xffffff);
            screen.Print("FOV: " + renderCam.fovv, 513, 5, 0xffffff);

            //screen plane
            screen.Line(TX(renderCam.p0.X) + 512, TY(renderCam.p0.Z), TX(renderCam.p1.X) + 512, TY(renderCam.p1.Z), 0xffffff);


            foreach (Primitive p in scene.Primitives)
            {
                if (p is Sphere)
                {
                    Sphere s = (Sphere)p;
                    Vector3 sphereColor = s.PrimitiveColor;
                    if (s.PrimitiveMaterial.isMirror)
                        sphereColor = new Vector3(255, 255, 255);

                    for (float r = 0; r < 10; r += .1f)
                    {
                        screen.Line(
                            TX((float)(s.PrimitivePosition.X + s.Radius * Math.Cos(r))) + 512,
                            TY((float)(s.PrimitivePosition.Z + s.Radius * Math.Sin(r))),
                            TX((float)(s.PrimitivePosition.X + s.Radius * Math.Cos(r + .1))) + 512,
                            TY((float)(s.PrimitivePosition.Z + s.Radius * Math.Sin(r + .1))),
                            CreateColor((int)sphereColor.X, (int)sphereColor.Y, (int)sphereColor.Z)
                            );
                    }

                }
                else if (p is Triangle)
                {
                    Triangle t = (Triangle)p;
                    Vector3 vert1 = t.v1 - t.v0;
                    Vector3 vert2 = t.v2 - t.v1;
                    //float labda = ;

                    screen.Line(
                        TX(t.v0.X) + 512,
                        TY(t.v0.Z),
                        TX(t.v2.X) + 512,
                        TY(t.v2.Z),
                        CreateColor((int)t.PrimitiveColor.X, (int)t.PrimitiveColor.Y, (int)t.PrimitiveColor.Z)
                        );

                }
            }
        }

        public void DrawDebugRay(Ray ray, Intersection I, int color)
        {
            //draws the primary ray in debug
            screen.Line(
                    TX(ray.O.X) + 512,
                    TY(ray.O.Z),
                    TX(ray.O.X + ray.D.X * I.Distance) + 512,
                    TY(ray.O.Z + ray.D.Z * I.Distance),
                    color
                        );


            //draws reflective rays
            if (I.Primitive != null)
            {
                if (I.IntersectPosition.X > xmax  || I.IntersectPosition.X < xmin || I.IntersectPosition.Z > ymax || I.IntersectPosition.Z < ymin) return;

                if(I.Primitive.PrimitiveMaterial.isMirror || I.Primitive.PrimitiveMaterial.isSpecular)
                {
                    bool wasMirror = I.Primitive.PrimitiveMaterial.isMirror;
                    Ray reflectRay = Reflect(ray, I);
                    Intersection newI = scene.closestIntersect(reflectRay);
                    DrawDebugRay(reflectRay, newI, 0x5f5f5f);

                    if (wasMirror) return;
                }

                if (I.Primitive.PrimitiveMaterial.isDiffuse || I.Primitive.PrimitiveMaterial.isSpecular)
                {
                    Ray shadowRay = new Ray();

                    foreach (Light l in scene.Lights)
                    {
                        shadowRay.D = (I.IntersectPosition - l.Position);
                        shadowRay.O = l.Position;
                        float length = Length(shadowRay.D);
                        shadowRay.Normalize();

                        Intersection newI = scene.closestIntersect(shadowRay);

                        if ((int)(length * 100) == (int)(newI.Distance * 100))
                            screen.Line(
                                TX(shadowRay.O.X) + 512,
                                TY(shadowRay.O.Z),
                                TX(shadowRay.O.X + shadowRay.D.X * newI.Distance) + 512,
                                TY(shadowRay.O.Z + shadowRay.D.Z * newI.Distance),
                                CreateColor((int)l.Color.X * 255, (int)l.Color.Y * 255, (int)l.Color.Z * 255)
                                        );
                    }              
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

        float Length(Vector3 vec)
        {
            return (float)Math.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z));
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