using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool1<T>
{
    private readonly Queue<T> _objectPool;
    private readonly Func<T> _createFunction;
    private readonly Action<T> _actionOnGet;
    private readonly Action<T> _actionOnReturn;

    public ObjectPool1(Func<T> createFunction, Action<T> actionOnGet = null, Action<T> actionOnReturn = null, int amount = 10)
    {
        _objectPool = new Queue<T>(amount);
        _createFunction = createFunction;
        _actionOnGet = actionOnGet;
        _actionOnReturn = actionOnReturn;
    }

    public T Get()
    {
        T obj;
        if(_objectPool.Count <= 0) obj = _createFunction();
        else obj = _objectPool.Dequeue();

        Action<T> actionOnGet = _actionOnGet;

        if (actionOnGet != null)
            actionOnGet.Invoke(obj);
        
        return obj;
    }

    public void Return(T element)
    {
        if (_objectPool.Contains(element))
            Debug.LogError("Trying to return an element that is already in the pool");

        Action<T> actionOnReturn = _actionOnReturn;
        if(actionOnReturn != null)
            actionOnReturn.Invoke(element);

        _objectPool.Enqueue(element);
    }
}
