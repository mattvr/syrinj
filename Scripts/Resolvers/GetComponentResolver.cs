using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;
using Syrinj.Exceptions;

namespace Syrinj.Resolvers
{
    public class GetComponentResolver : IResolver
    {
        public object Resolve(Injectable injectable)
        {
            var monoBehaviour = injectable.Object as MonoBehaviour;
            if (monoBehaviour == null)
            {
                throw new InjectionException(injectable.Object, "[GetComponent] annotation on a non-MonoBehaviour");
            }

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