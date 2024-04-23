	This package contains 3 classes; a generic base class `Pool<T>`, and two unity specific derivations MonoPool<T>, and GamObjectPool.
	This is an implementation of a generic object pool. An object pool is a container that holds a collection of objects that can be reused, rather than creating a new instance each time an object is needed. This can be useful for improving performance and reducing memory usage, as it can be more efficient to reuse objects rather than creating new ones.
	The 'Pool' class has several methods for adding objects back to the pool ('Toss'), and retrieving objects from the pool ('Pull'). 
	It also has two delegate properties onPull and onToss, which are called each time an object is retrieved from or added back to the pool. These are used by the `MonoPool<T>`, and `GamObjectPool` classes so they can enable/disable or activate/deactive objects as they are pulled and tossed from/into the pool.
	The class can be initialized with a default value for objects in the pool, and this default value will be used if the pool is empty when an object is requested.
    The class implements a list to store the objects in the pool, and it uses the RemoveAt method to retrieve the last item in the list each time an object is requested, ensuring that the most recently added object is used first.
    Overall, this implementation provides a basic object pool implementation that can be customized by defining custom actions for onPull and onToss and by providing a default value for objects in the pool.


