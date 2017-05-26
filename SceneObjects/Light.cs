using OpenTK;

namespace Application
{
    public class Light
    {
        Vector3 location, color;
        int intensity;

        public Light(Vector3 pos, Vector3 col, int intensity)
        {
            location = pos;
            color = col;
            this.intensity = intensity;
        }

    }
}
