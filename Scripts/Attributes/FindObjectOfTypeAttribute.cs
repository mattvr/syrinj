using System;

namespace Syrinj.Attributes
{
    public class FindObjectOfTypeAttribute : UnityInjectorAttribute
    {
        public Type ComponentType { get; private set; }

        public FindObjectOfTypeAttribute(Type componentType)
        {
            ComponentType = componentType;
        }
    }
}
