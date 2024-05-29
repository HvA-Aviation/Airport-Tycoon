using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private int _spawnAmount;
    [SerializeField] private GameObject _object;

    private GameObject _parent;

    private Queue<GameObject> _objectPool = new Queue<GameObject>();

    private void Awake()
    {
        _parent = new GameObject(_object.name);
        GenerateNewAmount();
    }  

    /// <summary>
    /// Call this function to generate a new amount of object in a pool
    /// </summary>
    private void GenerateNewAmount()
    {
        for (int i = 0; i < _spawnAmount; i++)
            ReturnObject(Instantiate(_object));        
    }

    /// <summary>
    /// Call this function to get an object out of the pool
    /// </summary>
    /// <returns>The first object of pool</returns>
    public GameObject GetObject()
    {
        if (_objectPool.Count <= 0)
            GenerateNewAmount();

        GameObject obj = _objectPool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Call this function to return an object to the pool
    /// </summary>
    /// <param name="gameObject">The object that needs to be returned</param>
    public void ReturnObject(GameObject gameObject)
    {
        if(gameObject.TryGetComponent<IPoolableObject>(out IPoolableObject poolableObject))
            poolableObject.ResetValues();

        gameObject.transform.parent = _parent.transform;
        gameObject.SetActive(false);
        _objectPool.Enqueue(gameObject);
    }
}
