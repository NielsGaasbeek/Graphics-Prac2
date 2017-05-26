using OpenTK;

namespace Application
{
    class Camera
    {
        Vector3 position, direction, screenCenter;
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

        public Vector3 Position
        {
            get { return position; }
        }
    }
}
