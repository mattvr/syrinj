using System;
using System.Collections.Generic;
using System.Reflection;
using Syrinj.Attributes;
using Syrinj.Caching;
using Syrinj.Exceptions;
using Syrinj.Resolvers;
using Syrinj.Graph;
using Syrinj.Injection;
using Syrinj.Providers;
using UnityEngine;

namespace Syrinj
{
    public class GameObjectInjector
    {
        private static readonly HashSet<GameObject> visited = new HashSet<GameObject>(); 

        private readonly GameObject gameObject;
        private readonly bool injectChildren;
        private readonly List<Injectable> injectables;

        public GameObjectInjector(GameObject gameObject) : this(gameObject, false)
        {

        }

        public GameObjectInjector(GameObject gameObject, bool injectChildren) 
        {
            this.gameObject = gameObject;
            this.injectables = new List<Injectable>();
            this.injectChildren = injectChildren;
        }

        public void Inject()
        {
            if (visited.Contains(gameObject)) return;

            var attachedBehaviours = GetAttachedBehaviours();
            DependencyContainer.Instance.Inject(attachedBehaviours);
            MarkAsVisited(attachedBehaviours);
        }

        private void MarkAsVisited(IList<MonoBehaviour> monoBehaviours)
        {
            for (int i = 0; i < monoBehaviours.Count; i++)
            {
                visited.Add(monoBehaviours[i].gameObject);
            }
        }

        private IList<MonoBehaviour> GetAttachedBehaviours()
        {
            if (injectChildren)
            {
                return gameObject.GetComponentsInChildren<MonoBehaviour>();
            }
            else
            {
                return gameObject.GetComponents<MonoBehaviour>();
            }
        }
    }
}