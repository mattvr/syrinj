using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Resolvers
{
    public class GetComponentResolver : IResolver
    {
        public object Resolve(Injectable injectable)
        {
            var attribute = (GetComponentAttribute) injectable.Attribute;
            if (attribute.ComponentType == null)
            {
                return injectable.MonoBehaviour.GetComponent(injectable.Type);
            }
            else
            {
                return injectable.MonoBehaviour.GetComponent(attribute.ComponentType);
            }
        }
    }
}