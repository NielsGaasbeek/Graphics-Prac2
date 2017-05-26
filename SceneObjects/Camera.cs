using OpenTK;

namespace Application
{
    class Camera
    {
        public Vector3 position, direction, screenCenter;
        public Vector3 p0, p1, p2; //screen corners
        float d = 1f; //for FOV adjustment

        public Camera(Vector3 position, Vector3 direction)
        {
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
            position = position + new Vector3(0.1f*up, 0.1f*right, 0.1f*away);
            p0 = p0 + new Vector3(0.1f * up, 0.1f * right, 0.1f * away);
            p1 = p1 + new Vector3(0.1f * up, 0.1f * right, 0.1f * away);
            p2 = p2 + new Vector3(0.1f * up, 0.1f * right, 0.1f * away);
        }

        public Vector3 Position
        {
            get { return position; }
        }
    }
}
