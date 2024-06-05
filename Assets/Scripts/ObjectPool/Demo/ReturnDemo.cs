using Features.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnDemo : MonoBehaviour, IPoolableObject
{
    [SerializeField] private ObjectPool _pool;   
    public void ResetValues()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            _pool.Return(gameObject);
    }
}
