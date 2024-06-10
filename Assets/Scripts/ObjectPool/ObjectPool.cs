using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour
{
    [SerializeField] private int _spawnAmount;
    [SerializeField] private GameObject _object;

    private GameObject _parent;

    private Queue<T> _objectPool;

    private Func<T> _createFunction;
    private Action<T> _getAction;
    private Action<T> _returnAction;

    public ObjectPool(Func<T> createFunction, Action<T> getAction, Action<T> returnAction, int spawnAmount)
    {
        _objectPool = new Queue<T> (spawnAmount);
        this._createFunction = createFunction;
        _getAction = getAction;
        _returnAction = returnAction;
    }

    /*private void Awake()
    {
        _parent = new GameObject(_object.name);
        FillObjectPool();
    }  */

    /// <summary>
    /// Call this function to generate a new amount of object in a pool
    /// </summary>
    /*private void FillObjectPool()
    {
        for (int i = 0; i < _spawnAmount; i++)
            Return(Instantiate(_object));        
    }*/

    /// <summary>
    /// Call this function to get an object out of the pool
    /// </summary>
    /// <returns>The first object of pool</returns>
    public T Get()
    {
        T obj;

        if (_objectPool.Count <= 0)
            obj = _createFunction();
        else
            obj = _objectPool.Dequeue();

        Action<T> getAction = this._getAction;
        if (getAction != null)
            getAction(obj);

        return obj;
    }

    /// <summary>
    /// Call this function to return an object to the pool
    /// </summary>
    /// <param name="gameObject">The object that needs to be returned</param>
    public void Return(T element)
    {
        if(gameObject.TryGetComponent<IPoolableObject>(out IPoolableObject poolableObject))
            poolableObject.ResetValues();

        gameObject.transform.parent = _parent.transform;
        gameObject.SetActive(false);
        _objectPool.Enqueue(element);
    }
}
