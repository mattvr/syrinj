#Syrinj
######**Lightweight dependency injection** & convenient attributes for Unity

###[Release 1.1.0 available here!](https://github.com/mfav/syrinj/releases/tag/1.1.0) (7/14/16)

---

##Table of Contents

* [Introduction](#introduction)
   * [Examples](#examples)
   * [Set-up](#set-up)
   * [What is this?](#what-is-this)
   * [Why use this?](#why-use-this)
* [Documentation](#documentation)
   * [Extended examples](#extended-examples)
   * [Convenience attributes](#convenience-attributes)
   * [Injection attributes](#injection-attributes)
* [Notes](#notes)
* [Troubleshooting](#troubleshooting)

---
##Introduction

####Examples
Convenience attributes:
```csharp
public class SimpleBehaviour : MonoBehaviour
{
    [GetComponent]  private Rigidbody rigidbody;
    [Find("Music")] private AudioSource musicSource;
}
```

Simple dependency injection:
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

####Set-up

1. Add `using Syrinj;` to the top of files which use Syrinj.

2. Annotate your classes with the attributes shown in the [Documentation](#documentation). 

3. Follow the steps below for your use case:


**For injection on scene load:**

Create a GameObject in your scene with the Component `SceneInjector`. 

**For injection while application is running:**

Attach the `InjectorComponent` to any GameObject which contains providers and injectors. 

Set the `ShouldInjectChildren` property in the inspector if you wish to inject children of the GameObject as well. DO NOT attach another InjectorComponent to those children. There should be only one root `InjectorComponent` for an object created with `GameObject.Instantiate()`.

**For providing non-MonoBehaviours:**

Add `[Instance]` or `[Singleton]` attributes to providers of non-MonoBehaviours. These will construct new instances or a shared single instance, respectively, at injection sites. 

**For injecting non-MonoBehaviours:**

Inject a `Provider<T>` if you wish to create your own injected objects. `T` is the object you wish to create, and must be a non-MonoBehaviour with a default constructor. Then call `Get()` on the provider for a new instance.

---

####What is this?

Syrinj is a small package to make creating objects simpler in Unity.

It provides convenient attributes, such as `[GetComponent]` which automatically tell your MonoBehaviours where to find their dependencies. 

For more customizable or shared dependencies, Syrinj allows you to specify providers and injection sites. See the [extended examples](#extended-examples) for how to do this. You can even mix attributes like `[GetComponent]` with a `[Provider]`, so that the `GetComponent()` method only runs once!

####Why use this?

If you're familiar with dependency injection and see how Syrinj could help your project, check out the [set-up](#set-up) and [documentation](#documentation) to see more. If not, read on:

Dependency injection is an intimidating word for a simple concept you're likely familiar with. It simply means if `ObjectA` creates `ObjectB`, then `ObjectA` resolves all of `ObjectsB`'s dependencies (i.e. fields & properties).

Here's a more concrete example. Say your enemies have a `RocketLauncher` which can fire homing missiles. I will call these `GoodHomingMissile` to denote that this is a *good* way to do this. Here is how you might fire a missile at the player:

```csharp
public class RocketLauncher {
    private Player player;

    public void Fire() {
        var target = player;
        var missile = new GoodHomingMissile(target);
    }
}

public class GoodHomingMissile {
    private Player target;

    public GoodHomingMissile(Player target) {
        this.target = target;
    }

    public void MoveTowardsTarget() {
        // ...
    }
}
```

Make sense? That's dependency injection. The `GoodHomingMissile` has a dependency of a `Player` target, and the `RocketLauncher` tells it which target to move towards on construction! This is a good practice because you know that if a `GoodHomingMissile` is created, it must have had its target specified. 

If you're a Unity developer, you may already notice a slight issue. In Unity, you don't instantiate objects with constructors! Instead, you call `GameObject.Instantiate()`. One workaround is to make an `Initialize()` method:

```csharp
public class OkayHomingMissile : MonoBehaviour {
    private Player target;

    public void Initialize(Player target) {
        this.target = target;
    }

    // ...
}
```

This is okay, but you lose the guarantee that the `OkayHomingMissile` has its dependencies right when it's created. You have to remember to call `Initialize()` every time. Let's complicate it further, and imagine the `OkayHomingMissile` also creates an `Explosion` when it reaches the player! The `Explosion` needs to know who to damage, and so it receives a `Player` as well when it's created.

```csharp
public class OkayHomingMissile : MonoBehaviour {
    // ...

    void Update() {
        if (distanceToTarget() < 0.1f) {
            var explosion = new Explosion();
            explosion.Initialize(target);
        }
    }
}
```

Now we've passed this `Player` object between three classes, and it's getting a bit difficult to keep track of. Plus, in reality your classes are going to have a lot more than one dependency. Can you imagine doing this?:

```csharp
explosion.Initialize(target, damage, radius, audioManager, particleManager, camera);
```

At this point, most Unity developers will settle on using Unity's inspector to set dependencies, and the infamous [Singleton](https://en.wikipedia.org/wiki/Singleton_pattern). Neither of these solutions are inherently bad, but they can lead to code that's difficult to maintain. A homing missile in practice may end up looking like this:

```csharp
public class BadHomingMissile : MonoBehaviour {
    public GameManager gameManager; // set in inspector
    public ParticleSystem particles; // set in inspector
    public AudioSource audio;
    private int damage;
    private Player target;
    private RocketLauncher launcher;

    void Start() {
        audio = this.GetComponent<AudioSource>();
        target = Player.Instance;
        launcher = RocketLauncher.Instance;
    }
}
```

There's a lot of issues with this:

- Your dependencies ought to be specified in code. You hope that the `GameManager` and `ParticleSystem` are set, but you may have drag-n-dropped the wrong object. Or you may have forgotten to do it all together.

- Homing missiles are now tightly coupled to Singletons (i.e. `Player.Instance` and `RocketLauncher.Instance`). How do you know the Singleton exists when `BadHomingMissile` calls `Start()`? What if later on you want more then one `RocketLauncher` or `Player`? Extending and maintaining your classes will take a lot more effort.

- These fields don't need to be public (easy solution: expose private fields in the inspector with `[SerializeField]`).

- If a dependency isn't met, you won't receive an informative error message about what happened.

I'd argue that the **most common issue with Unity code is bad dependency management and overuse of Singletons**. Syrinj addresses all of the problems mentioned in the above exmaple.

There are alternatives to Syrinj for Unity, such as [Zenject](https://github.com/modesttree/Zenject) and [StrangeIoC](http://strangeioc.github.io/strangeioc/). These are great at what they do and worth checking out if you're starting a new project. Unfortunately they can be bulky, difficult to implement in an existing project, and harder to approach. However, I'd highly recommend reading the authors' elaboration on dependency injection and inversion-of-control to become a better programmer.

Syrinj allows you to write fewer lines of code, not more. You can take advantage of as many or as few of Syrinj's features as you'd like. Check out the [set-up section](#set-up) to see how easy it is to get started.

---

##Documentation

####Extended examples
```csharp
public class ExampleProvider : MonoBehaviour
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

    [Provides("Primary")] // specify optional tags for multiple bindings of the same type
    public Camera PrimaryCamera;

    [Provides("Secondary")]
    public Camera SecondaryCamera;
  
  	[Provides] [Instance]
  	public NPC NPCProvider; // creates a new NPC at each injection site
  
  	[Provides] [Singleton]
  	public Player PlayerProvider; // shares the same Player each injection
}

// ...

public class ExampleInjectee : MonoBehaviour
{
    [Inject] private Canvas UIRoot;
    [Inject] private float RandomNumber;
    [Inject] private AudioSource MusicSource;
  
  	[Inject] private Provider<Enemy> Spawner; // instantiates new injected NPC on Spawner.Get();

    [Inject("Primary")]     private Camera primaryCamera;
    [Inject("Secondary")]   private Camera secondaryCamera;

    [GetComponent] 
    private Rigidbody rigidbody; // automatically caches Rigidbody on this object

    [FindWithTag("Player")]
    private GameObject Player { get; set; } // works with properties, as long as they can be set
}
```

#####Convenience attributes:

| Attribute                  | Arguments                          | Usage                                    |
| -------------------------- | ---------------------------------- | ---------------------------------------- |
| `[GetComponent]`           | *opt.* `System.Type ComponentType` | Gets a component attached to this GameObject. |
| `[GetComponentInChildren]` | *opt.* `System.Type ComponentType` | Gets a component attached to this GameObject or its children. |
| `[Find]`                   | `string GameObjectName`            | Finds a GameObject in scene with a given name. |
| `[FindWithTag]`            | `string Tag`                       | Finds a GameObject in scene with a given tag. |
| `[FindObjectOfType]`       | `System.Type ComponentType`        | Finds a component in the scene with a given type. |

#####Injection attributes:

| Attribute     | Arguments           | Usage                                    |
| ------------- | ------------------- | ---------------------------------------- |
| `[Provides]`  | *opt.* `string Tag` | Registers a provider for a given tag and type. |
| `[Inject]`    | *opt.* `string Tag` | Injects a field/property for a given tag and type. |
| `[Instance]`  | *none*              | Attach to `[Provides]` to construct a new instance at every injection. |
| `[Singleton]` | *none*              | Attach to `[Provides]` to construct a singleton instance shared across injections. |

#####Classes

| Class         | Usage                                    |
| ------------- | ---------------------------------------- |
| `Provider<T>` | Inject this class if you want to construct instances of `T` and have them be injected. Use `Get()` to construct a new instance of `T`, where `T` has a default constructor and is not a MonoBehaviour. |

##Notes

- Currently only active GameObjects are injected with [Inject] or the convenience attributes.

##Troubleshooting

**Q: My fields/properties aren't being injected**
  (or) I'm getting an error about missing dependency/provider/resolver

A: Follow these steps in order:

1. Make sure there the `[Inject]` attribute is on the proper injected field, and the `[Provides]` attribute is on the proper provider field. Ensure both are bound to the exact same `Tag` (or lack thereof) and `Type`.

2. Your injecting/providing GameObjects must have `InjectorComponent`s attached (if created with `GameObject.Instiantiate()`) and/or a `SceneInjector` component must exist *somewhere* in the scene (if object exists in the scene initially).

3. For every object that you call `GameObject.Instantiate()` on, you should have at most ONE `InjectorComponent`. Place this component at the root GameObject, with `ShouldInjectChildren` set if necessary. 

4. Make sure the fields or property providers aren't null! Use `Debug.Log()` and/or double-check the inspector for the object.

5. Verify the script execution order in Unity. Go to `Edit -> Project Settings -> Script Execution Order` and modify the `Syrinj.InjectorComponent` and `Syrinj.SceneInjcetor` scripts to execute **before** all other scripts. Put in a large negative number such that these two scripts before any others in the list.

6. There might be some other problem. Create an issue on GitHub/[message me](https://twitter.com/perceptron)/fix it yourself with a pull request!
