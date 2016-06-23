using Syrinj.Injection;
using UnityEngine;

namespace Syrinj
{
    public class ExtendedMonoBehaviour : MonoBehaviour {
        public virtual void Awake()
        {
            var injector = new MonoBehaviourInjector(this);
            injector.Inject();
        }
    }
}
