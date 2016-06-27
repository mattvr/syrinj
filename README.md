#Syrinj
######**Lightweight dependency injection** & convenient attributes for Unity

---

* [Examples](#examples)
* [Set-up](#set-up)
* [Extended examples](#extended-examples)
* [Documentation](#documentation)
    * [Convenience Attributes](#convenience-attributes)
    * [Injection Attributes](#injection-attributes)
* [Notes](#notes)

---
##Examples
####Convenience attributes:
```csharp
public class SimpleBehaviour : ExtendedMonoBehaviour
{
    [GetComponent]  private Rigidbody rigidbody;
    [Find("Music")] private AudioSource musicSource;
}
```

####Simple dependency injection:
```csharp
public class SceneProviders : MonoBehaviour 
{
    [Provides] 
    public Light SunProvider; // drag object in inspector to set
    
    [Provides]
    [FindObjectOfType(typeof(Player))]
    public Player PlayerProvider; // provides Player object from scene
}

// ...

public class SimpleBehaviour : MonoBehaviour
{
    [Inject] public Light Sun; 
    [Inject] public Player MyPlayer;
}
```

---

##Set-up

**For injection on scene load:**

Create a GameObject in your scene with the Component `SceneInjector`. 

**For injection during runtime:**

Attach the `InjectorComponent` to any GameObject which contains providers and injectors. Set the `ShouldInjectChildren` property in the inspector if you wish to inject children of the GameObject as well.

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

##Documentation

#####Convenience Attributes:

Attribute | Arguments | Usage
--- | --- | ---
`[GetComponent]`| *optional* `System.Type ComponentType` | Gets a component attached to this GameObject.
`[GetComponentInChildren]`| *optional* `System.Type ComponentType` | Gets a component attached to this GameObject or its children.
`[Find]` | `string GameObjectName` | Finds a GameObject in scene with a given name.
`[FindWithTag]` | `string Tag` | Finds a GameObject in scene with a given tag.
`[FindObjectOfType]` | `System.Type ComponentType` | Finds a component in the scene with a given type.

#####Injection Attributes:

| Attribute | Arguments | Usage |
| --- | --- | --- |
|`[Provides]`| *optional* `string Tag` | Registers a provider for a given tag and type. |
|`[Inject]`| *optional* `string Tag` | Injects a field/property for a given tag and type. |

##Notes

- Currently only active GameObjects are injected with [Inject] or the convenience attributes.
