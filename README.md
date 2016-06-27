#Syrinj
######**Lightweight dependency injection** & convenient attributes for Unity

---

* [Introduction](#introduction)
   * [Examples](#examples)
   * [What is this?](#what-is-this)
   * [Why use this?](#why-use-this)
* [Set-up](#set-up)
* [Documentation](#documentation)
   * [Extended examples](#extended-examples)
   * [Convenience attributes](#convenience-attributes)
   * [Injection attributes](#injection-attributes)
* [Notes](#notes)

---
##Introduction

####Examples
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

####What is this?

Syrinj is a small package to make creating objects simpler in Unity.

It provides convenient attributes, such as `[GetComponent]` which automatically tell your MonoBehaviours where to find their dependencies. 

For more customizable or shared dependencies, Syrinj allows you to specify providers and injection sites. See the [extended examples](#extended-examples) for how to do this. You can even mix attributes like `[GetComponent]` with a `[Provider]`, so that the `GetComponent<T>` method only runs once!

####Why use this?

One option is to use Unity's inspector to inject dependencies. This seems to be the most common approach. However, errors often emerge as you drag-n-drop a tangled web of MonoBehaviours around your scene. These dependencies ought to be specified in your code, not in an editor.

Another option is to use other DI/IoC frameworks made for Unity. These are often bulky or impractical. Other frameworks require you to write lots of factory classes and binding logic. You may even have to re-imagine your project's design to integrate these alternatives.

Syrinj is much simpler. No reworking of your entire codebase or throwaway code required. Simply annotate the fields and methods of your MonoBehaviours, and everything will be connected for you.

---

##Set-up

**For injection on scene load:**

Create a GameObject in your scene with the Component `SceneInjector`. 

**For injection during runtime:**

Attach the `InjectorComponent` to any GameObject which contains providers and injectors. Set the `ShouldInjectChildren` property in the inspector if you wish to inject children of the GameObject as well.

---

##Documentation

####Extended examples
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

#####Convenience attributes:

Attribute | Arguments | Usage
--- | --- | ---
`[GetComponent]`| *opt.* `System.Type ComponentType` | Gets a component attached to this GameObject.
`[GetComponentInChildren]`| *opt.* `System.Type ComponentType` | Gets a component attached to this GameObject or its children.
`[Find]` | `string GameObjectName` | Finds a GameObject in scene with a given name.
`[FindWithTag]` | `string Tag` | Finds a GameObject in scene with a given tag.
`[FindObjectOfType]` | `System.Type ComponentType` | Finds a component in the scene with a given type.

#####Injection attributes:

| Attribute | Arguments | Usage |
| --- | --- | --- |
|`[Provides]`| *opt.* `string Tag` | Registers a provider for a given tag and type. |
|`[Inject]`| *opt.* `string Tag` | Injects a field/property for a given tag and type. |

##Notes

- Currently only active GameObjects are injected with [Inject] or the convenience attributes.
