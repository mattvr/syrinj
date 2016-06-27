#Syrinj
######**Lightweight dependency injection** & convenient attributes for Unity

---

* [Examples](#Examples)
* [Set-up](#Set-up)
* [Extended examples](#Extended examples)
* [Notes](#Notes)

---
##Examples
**Convenience attributes:**
```csharp
public class SimpleBehaviour : ExtendedMonoBehaviour
{
    [GetComponent]  private Rigidbody rigidbody;
    [Find("Music")] private AudioSource musicSource;
}
```

**Simple dependency injection:**
```csharp
public class SceneProviders : MonoBehaviour 
{
    // these fields are providers/bindings for dependency injection
    
    [Provides] 
    public Light SunProvider; // drag object in inspector to set
    
    [Provides]
    [FindObjectOfType(typeof(Player))]
    public Player PlayerProvider; // provides Player object from scene
}

// ...

public class SimpleBehaviour : MonoBehaviour
{
    // these fields automatically inject on Awake()
    
    [Inject] public Light Sun; 
    [Inject] public Player MyPlayer;
}
```

---

##Set-up

**For injection on scene load:**

Create a GameObject in your scene with the Component `SceneInjector`. 

**For injection during runtime:**

Attach the `InjectorComponent` to any GameObject which contains providers and injectors. Set the "ShouldInjectChildren" property in the inspector if you wish to inject children of the GameObject as well.

---

##Extended examples
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
            return Random.RandomRange(0f, 1f); 
        } // define custom provider properties, these evaluate each injection
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
    
    [GetComponent] 
    private Rigidbody rigidbody; // automatically caches Rigidbody on this object
    
    [FindWithTag("Player")]
    private GameObject Player { get; set; } // works with properties, as long as they can be set
}
```

##Notes
