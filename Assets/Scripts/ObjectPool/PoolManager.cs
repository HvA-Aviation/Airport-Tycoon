using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<ObjectPool> _allObjectPools;


    public List<ObjectPool> AllObjectPools => _allObjectPools;
}
