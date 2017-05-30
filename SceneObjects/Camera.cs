using OpenTK;
using System;

namespace Application
{
    class Camera
    {
        public Vector3 position, camera_direction, screenCenter;
        public Vector3 up_direction, right_direction;
        public Vector3 p0, p1, p2; //screen corners
        public float fovv;
        public float FOV;
        float d;
        //for FOV adjustment               

        public Camera(Vector3 position, Vector3 direction, float fov)
        {
            //turning degrees into radians and using them to calculate the distance to screen with a given FOV.
            fovv = fov;
            FOV = fov*(float)Math.PI/180;
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
            //rotating the camera is done by adding or substracting the up- or right-vector based on what rotation is desired
            camera_direction += (right * right_direction);
            camera_direction += (up * up_direction);

            //after that, all the neccesary variables are recalculated
            CalcVars();
        }

        //this method calculates all variables needed for the camera to function properly
        public void CalcVars()
        {
            //a seperate variable for the normalized view-direction is calculated
            Vector3 normalized_direction = Normalize(camera_direction);

            //which is then used to calculate the screen center
            screenCenter = position + d * normalized_direction;
            
            //up- and right-vectors, used for many of the calculations and translations
            right_direction = Vector3.Cross(normalized_direction, up_direction);
            right_direction = Normalize(right_direction);
            up_direction = Vector3.Cross(right_direction, normalized_direction);
            up_direction = Normalize(up_direction);

            //corners of the screen-plane are then calculated using the screen center and up- and right-vectors
            p0 = screenCenter + -1 * right_direction + 1 * up_direction;
            p1 = screenCenter + 1 * right_direction + 1 * up_direction;
            p2 = screenCenter + -1 * right_direction + -1 * up_direction;
        }

        Vector3 Normalize(Vector3 v)
        {
            float length = (float)Math.Sqrt((v.X * v.X) + (v.Y * v.Y) + (v.Z * v.Z));
            return v / length;
        }

    }
}
