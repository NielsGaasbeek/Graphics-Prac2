using OpenTK;

namespace Application
{
    public class Light
    {
        Vector3 location, color;

        public Light(Vector3 pos, Vector3 col)
        {
            location = pos;
            color = col;
        }

    }
}
