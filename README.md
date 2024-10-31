# Pool Repository

This package includes a C# implementation of a generic object pool, a data structure and object factory designed to help improve performance and reduce memory usage by reusing objects instead of constantly creating new ones. This can significantly boost the efficiency of application memory usage. Inculdes class made specifically for use with Unity types and classes.

## Features

This package contains the following classes:

- `Pool<T>`: A generic base class for object pooling.
- `MonoPool<T>`: A Unity-specific derivation that handles MonoBehaviour objects. Optionally enables/disabled componet on pull/toss.
- `GamObjectPool`: Another Unity-specific derivation tailored for GameObjects. Optionally activates/deactivaes GameObject on pull/toss.

### Dependencies

- .NET Framework
- Unity

### Installation

Install this package in your Unity project using the Package Manager:

1. Open the Package Manager window (**Packages** > **Manage Packages**).
2. Click on the **+** button in the top left corner and select **Add package from git URL**.
3. Paste the following URL into the address field and click **Install**: https://github.com/glurth/SerializableType.git

### Key Functionalities

- **Toss**: Adds objects back to the pool.
- **Pull**: Retrieves objects from the pool.

Additionally, it features two delegates that can, optionally, be assigned by via a the constrctor that takes them as parameters:

- **onPull**: Triggered when an object is retrieved from the pool.
- **onToss**: Triggered when an object is added back to the pool.

These delegates allow for custom behaviors such as enabling/disabling or activating/deactivating objects each time they are pulled from or tossed into the pool, this can be particularly useful in Unity game development.  They may also be accessed as protected members when creating custom decendants of the Pool<T> class.

### Implementation Details

- The pool uses a list to store the pooled objects.
- Objects are retrieved from the pool using the `RemoveAt` method, which pulls the last item in the list to ensure that the most recently added object is used first.
- The class can be initialized with a default value for objects in the pool, which is used if the pool is empty when an object is requested.

## Usage

To use this object pool, include it in your project and instantiate it with the specific type you need. Define any custom actions for `onPull` and `onToss` to suit your application's requirements, and set an initial default value if necessary.

```csharp
using EyE.Collections;
.
.
.
Pool<MyObject> myPool = new Pool<BulletClass>(default(MyObject));
//OR set code to run on pull or toss
Pool<MyObject> myPool = new Pool<MyObject>(
	myObjectDefaultValueMember,
	(obj) => { /* activation code here */ },
	(obj) => { /* deactivation code here */ });
...
```
Another Example, Using a custom Unity Monobehavior 
```csharp
using EyE.Collections;
.
.
.
MonoPool<BulletClass> myBulletPool = new MonoPool<BulletClass>(
	bulletPreFabRef, bulletSpaceTransform,
	(bullet) => { bullet.gameObject.SetActive(true); bullet.OnFired();}, //on pull
	(bullet) => { bullet.OnHit(); bullet.gameObject.SetActive(false);});); //on toss

BulletClass ShootBullet(Transform gunMuzzel)
{
	BulletClass b = myBulletPool.Pull();
	b.transform.position = gunMuzzel.position;
	b.transform.rotation = gunMuzzel.rotation;
	b.velocity = gunVelocityMember;
	..stuff..
	return b;
}

void OnBulletHit(BulletClass b)
{
	...stuff...
	myBulletPool.Toss(b);
}
