using System.Collections.Generic;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj
{
    public class GameObjectInjector
    {
        private static readonly HashSet<GameObject> visited = new HashSet<GameObject>(); 

        private readonly GameObject gameObject;
        private readonly bool injectChildren;

        public GameObjectInjector(GameObject gameObject) : this(gameObject, false)
        {

        }

        public GameObjectInjector(GameObject gameObject, bool injectChildren) 
        {
            this.gameObject = gameObject;
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