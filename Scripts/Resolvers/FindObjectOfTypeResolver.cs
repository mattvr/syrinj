using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Resolvers
{
    public class FindObjectOfTypeResolver : IResolver
    {
        public object Resolve(Injectable injectable)
        {
            var attribute = (FindObjectOfTypeAttribute)injectable.Attribute;
            if (attribute.ComponentType == null)
            {
                return Object.FindObjectOfType(injectable.Type);
            }
            else
            {
                return Object.FindObjectOfType(attribute.ComponentType);
            }
        }
    }
}