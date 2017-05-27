using OpenTK;

namespace Application
{
    public class Light
    {
        Vector3 position, color;
        int intensity;

        public Light(Vector3 pos, Vector3 col, int intensity)
        {
            position = pos;
            color = col;
            this.intensity = intensity;
        }

        public Vector3 Position
        { get { return position; } }
        public Vector3 Color
        { get { return color; } }
        public int Intensity
        { get { return intensity; } }
    }
}
