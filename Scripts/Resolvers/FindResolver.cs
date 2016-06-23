using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Resolvers
{
    public class FindResolver : IResolver
    {
        public object Resolve(MonoBehaviour monoBehaviour, Injectable injectable)
        {
            return GameObject.Find(((FindAttribute) injectable.Attribute).GameObjectName);
        }
    }
}