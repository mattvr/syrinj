using System;

namespace Syrinj.Attributes
{
    public class FindObjectOfTypeAttribute : UnityInjectorAttribute
    {
        public readonly Type ComponentType;

        public FindObjectOfTypeAttribute()
        {
            
        }

        public FindObjectOfTypeAttribute(Type componentType)
        {
            ComponentType = componentType;
        }
    }
}
