using Features.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnDemo : MonoBehaviour, IPoolableObject
{
    public void ResetValues()
    {
        Debug.Log("Test");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            GameManager.Instance.PoolManager.AllObjectPools[0].ReturnObject(gameObject);
    }
}
