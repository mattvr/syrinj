using System;

namespace Syrinj.Attributes
{
    public class GetComponentInChildrenAttribute : UnityInjectorAttribute
    {
        public Type ComponentType { get; private set; }

        public GetComponentInChildrenAttribute()
        {

        }

        public GetComponentInChildrenAttribute(Type componentType)
        {
            ComponentType = componentType;
        }
    }
}
