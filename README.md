#Syrinj
######*Unity Helper Attributes*

---

**Providing fields in Unity3D (without Syrinj):**
```csharp
public class Test : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Collider collider;
    
    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
}
```

**New way (with Syrinj):**
```csharp
public class Test : ExtendedMonoBehaviour
{
    [GetComponent] private Rigidbody rigidbody;
    [GetComponent] private Collider collider;
}
```

**Explanation:**

This framework simplifies some common tasks when constructing MonoBehaviours, without performance overhead or significant set-up. 

**Usage:**

Have your Unity classes inherit from ExtendedMonoBehaviour. Then use the documented annotations to automatically inject your Unity dependencies.

The annotations are evaluated in `ExtendedMonoBehaviour.Awake()`, so don't forget to call `base.Awake()` if you override Unity's `Awake()` functionality.

**Extended usage:**

```csharp
public class Test : ExtendedMonoBehaviour
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

**TODO:**
* Add tests!
* More informative error messages
* Cache member attributes
