using OpenTK;

namespace Application
{
    class Light
    {
        Vector3 location;
        float red, blue, green;

        public Light(Vector3 pos, float red, float green, float blue)
        {
            location = pos;

            this.red = red;
            this.green = green;
            this.blue = blue;
        }

    }
}
