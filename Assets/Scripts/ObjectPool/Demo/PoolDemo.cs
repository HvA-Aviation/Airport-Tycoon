using Features.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolDemo : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            GameManager.Instance.PoolManager.GetObjectOuOfPool("Test");
    }
}
