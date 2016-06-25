using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Resolvers
{
    public class FindResolver : IResolver
    {
        public object Resolve(Injectable injectable)
        {
            var name = ((FindAttribute)injectable.Attribute).GameObjectName;
            var gameObject = GameObject.Find(name);

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