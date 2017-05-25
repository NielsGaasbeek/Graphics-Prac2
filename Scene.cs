using OpenTK;
using System.Collections.Generic;

namespace Application
{
    class Scene
    {
        protected List<Light> lightList = new List<Light>();
        protected List<Primitive> sceneObjects = new List<Primitive>();

        public Scene()
        {

        }

        public void Intersect(Ray R)
        {


        }

        public IList<Light> Lights
        {
            get { return lightList; }
        }

        public IList<Primitive> Primitives
        {
            get { return sceneObjects; }
        }
    }
}
