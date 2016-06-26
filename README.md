#Syrinj
######**Lightweight dependency injection** & convenient attributes for Unity

---

**Convenience attributes:**
```csharp
public class SimpleBehaviour : ExtendedMonoBehaviour
{
    [GetComponent] private Rigidbody rigidbody;
    [GetComponent] private Collider collider;
}
```

**Simple dependency injection:**
```csharp
public class SceneProviders : ExtendedMonoBehaviour 
{
    // these fields are providers/bindings for dependency injection
    
    [Provides] 
    public Light SunProvider; // drag object in inspector to set
    
    [Provides]
    [FindObjectOfType(typeof(Player)]
    public Player PlayerProvider; // provides Player object from scene
}

// ...

public class SimpleBehaviour : ExtendedMonoBehaviour
{
    [Inject] public Light Sun; // automatically injects at runtime!
    [Inject] public Player MyPlayer;
}
```

####Explanation:

This framework simplifies dependency injection, without significant performance overhead or set-up. 

####Usage:

Have your Unity classes inherit from ExtendedMonoBehaviour. Then use the documented annotations to automatically inject your Unity dependencies.

---

####Extended usage of convenience attributes:

```csharp
public class ExampleAttributeUser : ExtendedMonoBehaviour
{
    [GetComponent] 
    private Rigidbody rigidbody; // automatically caches Rigidbody on this object

    [GetComponent(typeof(CapsuleCollider))]
    public Collider ParticularCollider;

    [Find("Canvas")]
    public GameObject UIRoot;
    
    [FindWithTag("Player")]
    private GameObject Player { get; set; } // works with properties, as long as they can be set
    
    [FindObjectOfType(typeof(Camera))]
    private Camera Camera;

    [GetComponentInChildren]
    private AudioSource ChildAudioSource;
}
```

####Extended usage of dependency injection:
```csharp
public class ExampleProvider : ExtendedMonoBehaviour
{
    [Provides]
    [FindObjectOfType(typeof(Canvas))]
    private Canvas UIRootProvider; // any convenience attribute can be combined with "Provides"
    
    [Provides]
    public float RandomNumberProvider 
    {
        get {
            return Random.Range(0, 1); // define custom provider properties, this will evaluate each injection
        }
    }
    
    [Provides]
    public AudioSource MusicSourceProvider; // manually set in inspector
}

// ...

public class ExampleInjectee : ExtendedMonoBehaviour
{
    // each field will be set on Awake()
    [Inject] private Canvas UIRoot;
    [Inject] private float RandomNumber;
    [Inject] private AudioSource MusicSource;
}
```

---
####*Notes:*

The annotations are evaluated in `ExtendedMonoBehaviour.Awake()`, so don't forget to call `base.Awake()` if you override Unity's `Awake()` functionality.

If you don't wish to use the `ExtendedMonoBehaviour` class, you just need to call: 

```csharp 
new MonoBehaviourInjector(this).Inject()
```

from your MonoBehaviour (replace `this` with `myMonoBehaviour` if called externally) when you want the annotations to be evaluated.

---

####TODO:
* Add tests!
* Implement dependency graph
