using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Resolvers
{
    public class GetComponentInChildrenResolver : IResolver
    {
        public object Resolve(MonoBehaviour monoBehaviour, Injectable injectable)
        {
            var attribute = (GetComponentInChildrenAttribute) injectable.Attribute;
            if (attribute.ComponentType == null)
            {
                return monoBehaviour.GetComponentInChildren(injectable.Type);
            }
            else
            {
                return monoBehaviour.GetComponentInChildren(attribute.ComponentType);
            }
        }
    }
}