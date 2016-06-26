using System;

namespace Syrinj.Attributes
{
    public class GetComponentInChildrenAttribute : UnityInjectorAttribute
    {
        public readonly Type ComponentType;

        public GetComponentInChildrenAttribute()
        {

        }

        public GetComponentInChildrenAttribute(Type componentType)
        {
            ComponentType = componentType;
        }
    }
}
