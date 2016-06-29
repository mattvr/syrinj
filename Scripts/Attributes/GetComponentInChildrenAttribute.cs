using System;
using Syrinj.Attributes;

namespace Syrinj
{
    public class GetComponentInChildrenAttribute : UnityConvenienceAttribute
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
