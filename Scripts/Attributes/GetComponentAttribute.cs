using System;

namespace Syrinj.Attributes
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
