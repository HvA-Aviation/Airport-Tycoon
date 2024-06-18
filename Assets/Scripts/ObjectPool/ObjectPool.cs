using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    private readonly Queue<T> _objectPool;
    private readonly Func<T> _createFunction;
    private readonly Action<T> _actionOnGet;
    private readonly Action<T> _actionOnReturn;

    private readonly int _spawnAmount;
    public ObjectPool(Func<T> createFunction, Action<T> actionOnGet = null, Action<T> actionOnReturn = null, int amount = 10)
    {
        _objectPool = new Queue<T>(amount);
        _createFunction = createFunction;
        _actionOnGet = actionOnGet;
        _actionOnReturn = actionOnReturn;
        _spawnAmount = amount;
    }

    /// <summary>
    /// Call this function when you want to get an object out of the pool
    /// </summary>
    /// <returns>Returns a single object out of the pool</returns>
    public T Get()
    {
        T obj;
        if (_objectPool.Count <= 0)
        {
            for (int i = 0; i < _spawnAmount; i++)
                _objectPool.Enqueue(_createFunction());
        }

        obj = _objectPool.Dequeue();

        Action<T> actionOnGet = _actionOnGet;

        if (actionOnGet != null)
            actionOnGet.Invoke(obj);

        return obj;
    }

    /// <summary>
    /// Call this function when you want to return an object to the pool
    /// </summary>
    /// <param name="element">The object that needs to be returned to the pool</param>
    public void Return(T element)
    {
        if (_objectPool.Contains(element))
            Debug.LogError("Trying to return an element that is already in the pool");

        Action<T> actionOnReturn = _actionOnReturn;
        if (actionOnReturn != null)
            actionOnReturn.Invoke(element);

        _objectPool.Enqueue(element);
    }
}
