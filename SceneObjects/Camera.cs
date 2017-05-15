using OpenTK;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Application
{
    class Camera
    {
        Vector3 position, direction;
        Surface Screenplane;

        public Camera(Vector3 position, Vector3 direction, Surface Screenplane)
        {
            this.position = position;
            this.direction = direction;
            this.Screenplane = Screenplane;         
        }

    }
}
