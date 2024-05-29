using Features.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnDemo : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            GameManager.Instance.PoolManager.ReturnObjectToPool("Test", gameObject);
    }
}
