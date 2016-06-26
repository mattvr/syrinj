using System;

namespace Syrinj.Attributes
{
    public class GetComponentAttribute : UnityInjectorAttribute
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
