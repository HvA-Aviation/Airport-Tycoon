using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<PoolableObject> _poolableObjects = new List<PoolableObject>();

    private Dictionary<string, Queue<GameObject>> _objectPool = new Dictionary<string, Queue<GameObject>>();
    private void Awake()
    {
        for(int i = 0; i < _poolableObjects.Count; i++)
        {
            Queue<GameObject> tempQueue = new Queue<GameObject>();
            GameObject parent = new GameObject(_poolableObjects[i].Name);

            for (int j = 0; j < _poolableObjects[i].AmountToSpawn; j++)
            {
                GameObject tempOBJ = Instantiate(_poolableObjects[i].Obj, parent.transform);
                tempOBJ.SetActive(false);

                tempQueue.Enqueue(tempOBJ);
            }

            _objectPool.Add(_poolableObjects[i].Name, tempQueue);
        }    
    }

    public void GetObjectOuOfPool(string name)
    {

    }


    public void ReturnObjectToPool(string name)
    {

    }

    [System.Serializable]
    public struct PoolableObject
    {
        public string Name;
        public GameObject Obj;
        public int AmountToSpawn;
    }
}
