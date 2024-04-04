using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
    private readonly T[] _environments;

    private readonly Func<T, T> CreateObject;
    private readonly Action<T> DisableObject;
    private readonly Action<T> EnableObject;

    private Queue<T> _pool = new();
    private List<T> _active = new();

    public ObjectPool(T[] environments, Func<T, T> createObject, Action<T> enableObject, Action<T> disableObject)
    {
        _environments = environments;
        CreateObject = createObject;
        EnableObject = enableObject;
        DisableObject = disableObject;

        for (int i = 0; i < _environments.Length; i++)
            Return(CreateObject(_environments[i]));
    }

    public T Get()
    {
        T obj = _pool.Count < 1 ? CreateObject(_environments[UnityEngine.Random.Range(0, _environments.Length - 1)]) : _pool.Dequeue();
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
