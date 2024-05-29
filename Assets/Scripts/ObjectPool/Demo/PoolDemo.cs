using Features.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolDemo : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
           GameManager.Instance.PoolManager.AllObjectPools[0].GetObject();
        }
    }
}
