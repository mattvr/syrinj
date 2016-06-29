using System;
using Syrinj.Attributes;

namespace Syrinj
{
    public class FindObjectOfTypeAttribute : UnityConvenienceAttribute
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
