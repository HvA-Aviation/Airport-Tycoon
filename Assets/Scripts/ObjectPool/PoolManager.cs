using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<SinglePoolableObject> _poolableObjects = new List<SinglePoolableObject>();

    private Dictionary<string, Queue<GameObject>> _objectPools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> _singleObjectPool = new Dictionary<string, GameObject>();
    private void Awake()
    {
        for(int i = 0; i < _poolableObjects.Count; i++)
        {
            Queue<GameObject> tempQueue = new Queue<GameObject>();
            GameObject parent = new GameObject(_poolableObjects[i].Name);

            _singleObjectPool.Add(_poolableObjects[i].Name, _poolableObjects[i].Obj);
            _singleObjectPool[parent.name].transform.parent = parent.transform;

            for (int j = 0; j < _poolableObjects[i].AmountToSpawn; j++)
            {
                GameObject tempOBJ = Instantiate(_poolableObjects[i].Obj, parent.transform);
                tempOBJ.SetActive(false);

                tempQueue.Enqueue(tempOBJ);
            }

            _objectPools.Add(_poolableObjects[i].Name, tempQueue);
        }    
    }

    /// <summary>
    /// Call this function when you want to have an object out of the pool
    /// </summary>
    /// <param name="name">The name of the pool you want to get the object out of</param>
    public void GetObjectOuOfPool(string name)
    {
        if (!_objectPools.ContainsKey(name))
            return;

        if (_objectPools[name].Count <= 0)
            for (int i = 0; i < 20; i++)
            {
                GameObject temp = Instantiate(_singleObjectPool[name], _singleObjectPool[name].transform.parent);
                temp.SetActive(false);
                _objectPools[name].Enqueue(temp);
            }

        GameObject tempObj = _objectPools[name].Dequeue();
        tempObj.SetActive(true);
    }

    /// <summary>
    /// Call this function when you want an object to return to the object pool
    /// </summary>
    /// <param name="name">The queue name that the object needs to return to</param>
    /// <param name="obj">The object that needs to be returned to the queue</param>
    public void ReturnObjectToPool(string name, GameObject obj)
    {
        if (!_objectPools.ContainsKey(name))
            return;

        obj.SetActive(false);

        _objectPools[name].Enqueue(obj);
    }

    [System.Serializable]
    public struct SinglePoolableObject
    {
        public string Name;
        public GameObject Obj;
        public int AmountToSpawn;
    }
}
