using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.Core.ExtensionMethods
{
    public static class Vector2fEM
    {     
        public static Vector2 ToVec2(this Vector2f vec2f) 
            =>  new(vec2f.X, vec2f.Y);
        
    }
}
