using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Resolvers
{
    public class FindWithTagResolver : IResolver
    {
        public object Resolve(MonoBehaviour monoBehaviour, Injectable injectable)
        {
            return GameObject.FindWithTag(((FindWithTagAttribute) injectable.Attribute).Tag);
        }
    }
}