using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Resolvers
{
    public class GetComponentResolver : IResolver
    {
        public object Resolve(MonoBehaviour monoBehaviour, Injectable injectable)
        {
            var attribute = (GetComponentAttribute) injectable.Attribute;
            if (attribute.ComponentType == null)
            {
                return monoBehaviour.GetComponent(injectable.Type);
            }
            else
            {
                return monoBehaviour.GetComponent(attribute.ComponentType);
            }
        }
    }
}