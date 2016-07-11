using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;
using Syrinj.Exceptions;

namespace Syrinj.Resolvers
{
    public class FindWithTagResolver : IResolver
    {
        public object Resolve(Injectable injectable)
        {
            var tag = ((FindWithTagAttribute) injectable.Attribute).Tag;
            var gameObject = GameObject.FindWithTag(tag);

            if (gameObject == null)
            {
                return null;
            }
            else if (injectable.Type != typeof(GameObject))
            {
                return gameObject.GetComponent(injectable.Type);
            }
            else
            {
                return gameObject;
            }
        }
    }
}