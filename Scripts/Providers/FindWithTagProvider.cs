using Syrinj.Attributes;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Providers
{
    public class FindWithTagProvider : IProvider
    {
        public object Provide(Injectable injectable)
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