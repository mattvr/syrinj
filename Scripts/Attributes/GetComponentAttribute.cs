using System;
using Syrinj.Attributes;

namespace Syrinj
{
    public class GetComponentAttribute : UnityConvenienceAttribute
    {
        public readonly Type ComponentType;

        public GetComponentAttribute()
        {
        
        }

        public GetComponentAttribute(Type componentType)
        {
            ComponentType = componentType;
        }
    }
}
