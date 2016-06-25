using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Providers
{
    public class FindObjectOfTypeProvider : IProvider
    {
        public object Provide(Injectable injectable)
        {
            return Object.FindObjectOfType(((FindObjectOfTypeAttribute) injectable.Attribute).ComponentType);
        }
    }
}