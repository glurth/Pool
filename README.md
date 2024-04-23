# Pool Repository

Welcome to the `Pool` repository! This package includes an efficient implementation of a generic object pool designed to help improve performance and reduce memory usage by reusing objects instead of constantly creating new ones. This can significantly boost the efficiency of applications, especially those developed in Unity.

## Features

This package contains the following classes:

- `Pool<T>`: A generic base class for object pooling.
- `MonoPool<T>`: A Unity-specific derivation that handles MonoBehaviour objects. Optionally enables/disabled componet on pull/toss.
- `GamObjectPool`: Another Unity-specific derivation tailored for GameObjects. Optionally activates/deactivaes GameObject on pull/toss.

### Key Functionalities

- **Toss**: Adds objects back to the pool.
- **Pull**: Retrieves objects from the pool.

Additionally, it features two delegate properties:

- **onPull**: Triggered when an object is retrieved from the pool.
- **onToss**: Triggered when an object is added back to the pool.

These delegates allow for custom behaviors such as enabling/disabling or activating/deactivating objects each time they are pulled from or tossed into the pool, making this tool particularly useful for Unity game development.

### Implementation Details

- The pool uses a list to store the pooled objects.
- Objects are retrieved from the pool using the `RemoveAt` method, which pulls the last item in the list to ensure that the most recently added object is used first.
- The class can be initialized with a default value for objects in the pool, which is used if the pool is empty when an object is requested.

## Usage

To use this object pool, include it in your project and instantiate it with the specific type you need. Define any custom actions for `onPull` and `onToss` to suit your application's requirements, and set an initial default value if necessary.

```csharp
Pool<MyObject> myPool = new Pool<MyObject>(default(MyObject));
//set code to run on pull or toss
Pool<MyObject> myPool = new Pool<MyObject>(
	default(MyObject),
	(obj) => { /* activation code here */ },
	(obj) => { /* deactivation code here */ });

