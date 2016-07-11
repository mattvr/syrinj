using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;
using Syrinj.Exceptions;

namespace Syrinj.Resolvers
{
    public class GetComponentInChildrenResolver : IResolver
    {
        public object Resolve(Injectable injectable)
        {
            var monoBehaviour = injectable.Object as MonoBehaviour;
            if (monoBehaviour == null)
            {
                throw new InjectionException(injectable.Object, "[GetComponentInChildren] annotation on a non-MonoBehaviour");
            }

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