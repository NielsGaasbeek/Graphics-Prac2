using OpenTK;
using System;

namespace Application
{
    class Camera
    {
        public Vector3 position, direction, screenCenter;
        public Vector3 p0, p1, p2; //screen corners
        public float FOV;
        float d;
        //for FOV adjustment
        
        

        public Camera(Vector3 position, Vector3 direction, float fov)
        {
            FOV = fov*(float)Math.PI/180;
            d = 1 / (float)(Math.Tan(FOV / 2));

            this.position = position;
            this.direction = direction;

            //screen plane, adjust FOV by altering d
            screenCenter = position + (d * direction);

            //corners, transform camera by multiplying E,p0,1,2 with camera matrix
            p0 = screenCenter + new Vector3(-1, -1, 0); //Only if direction (0,0,1)
            p1 = screenCenter + new Vector3(1, -1, 0); //Only if direction (0,0,1)
            p2 = screenCenter + new Vector3(-1, 1, 0); //Only if direction (0,0,1)
            
        }
        public void transform(float up, float right, float away)
        {
            //offsetting the position of the camera and the screen corners
            position = position + new Vector3(0.1f*up, 0.1f*right, 0.1f*away);
            p0 = p0 + new Vector3(0.1f * up, 0.1f * right, 0.1f * away);
            p1 = p1 + new Vector3(0.1f * up, 0.1f * right, 0.1f * away);
            p2 = p2 + new Vector3(0.1f * up, 0.1f * right, 0.1f * away);
        }
        public void rotate()
        {

        }
        
           
        
        
        public Vector3 Position
        {
            get { return position; }
        }
    }
}
