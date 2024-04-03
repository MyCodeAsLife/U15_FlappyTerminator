using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
    private readonly Func<T> CreateObject;
    private readonly Action<T> DisableObject;
    private readonly Action<T> EnableObject;

    private Queue<T> _pool;
    private List<T> _active;

    public ObjectPool(Func<T> createObject, Action<T> enableObject, Action<T> disableObject, int startCount)
    {
        CreateObject = createObject;
        EnableObject = enableObject;
        DisableObject = disableObject;

        for (int i = 0; i < startCount; i++)
            Return(createObject());
    }

    public T Get()
    {
        T obj = _pool.Count < 0 ? CreateObject() : _pool.Dequeue();
        EnableObject(obj);

        return obj;
    }
    public void Return(T obj)
    {
        DisableObject(obj);
        _pool.Enqueue(obj);
    }

    public void ReturnAll()
    {
        foreach (T obj in _active.ToArray())
            Return(obj);
    }
}
