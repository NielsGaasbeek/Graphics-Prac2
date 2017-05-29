using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    class Material
    {
        public bool isDiElectric, isMirror, isSpecular, isDiffuse;
        public Material(string mat = "")
        {
            switch (mat)
            {
                case "Mirror":
                    isMirror = true; break;
                case "DiElectric":
                    isDiElectric = true; break;
                case "Specular":
                    isSpecular = true; break;
                default:
                    isDiffuse = true; break;
            }
        }

    }
}
