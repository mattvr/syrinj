using UnityEngine;

namespace Syrinj
{
    public class RuntimeInjectableMonoBehaviour : MonoBehaviour {
        public virtual void Awake()
        {
            new GameObjectInjector(gameObject).Inject();
        }
    }
}
