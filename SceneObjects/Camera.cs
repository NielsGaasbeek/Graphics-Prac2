using OpenTK;
using System;

namespace Application
{
    class Camera
    {
        public Vector3 position, camera_direction, screenCenter;
        public Vector3 up_direction, right_direction;
        public Vector3 p0, p1, p2; //screen corners
        float FOV;
        float d;
        //for FOV adjustment               

        public Camera(Vector3 position, Vector3 direction, float fov)
        {
            FOV = fov * (float)Math.PI / 180;
            d = 1 / (float)(Math.Tan(FOV / 2));

            this.position = position;
            camera_direction = Normalize(direction);

            screenCenter = position + (d * camera_direction);

            up_direction = new Vector3(0, -1, 0);
            CalcVars();
        }

        public void transform(float up, float right, float away)
        {
            //offsetting the position of the camera and the screen corners
            position = position + new Vector3(0.05f * up, 0.05f * right, 0.05f * away);
            p0 = p0 + new Vector3(0.05f * up, 0.05f * right, 0.05f * away);
            p1 = p1 + new Vector3(0.05f * up, 0.05f * right, 0.05f * away);
            p2 = p2 + new Vector3(0.05f * up, 0.05f * right, 0.05f * away);
        }
        public void rotate(float up, float right)
        {
            /*
            camera_direction.Y += up;
            camera_direction.X += right;
            */

            /*
            theta += up;
            phi += right;
            */
            camera_direction += (right * right_direction);
            camera_direction += (up * up_direction);

            CalcVars();
        }


        public void CalcVars()
        {
            Vector3 normalized_direction = Normalize(camera_direction);

            screenCenter = position + d * normalized_direction;

            right_direction = RayTracer.CrossProduct(normalized_direction, up_direction);
            right_direction = Normalize(right_direction);
            up_direction = RayTracer.CrossProduct(right_direction, normalized_direction);
            up_direction = Normalize(up_direction);

            //corners, transform camera by multiplying E,p0,1,2 with camera matrix
            p0 = screenCenter + -1 * right_direction + 1 * up_direction; //Only if direction (0,0,1)
            p1 = screenCenter + 1 * right_direction + 1 * up_direction; //Only if direction (0,0,1)
            p2 = screenCenter + -1 * right_direction + -1 * up_direction; //Only if direction (0,0,1)
        }

        Vector3 Normalize(Vector3 v)
        {
            float length = (float)Math.Sqrt((v.X * v.X) + (v.Y * v.Y) + (v.Z * v.Z));
            return v / length;
        }

        public Vector3 Position
        {
            get { return position; }
        }
    }
}
