using UnityEngine;

public class ExtendedMonoBehaviour : MonoBehaviour {
    public virtual void Awake()
    {
        var injector = new MonoBehaviourInjector(this);
        injector.Inject();
    }
}
