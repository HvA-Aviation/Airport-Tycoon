using System;
using UnityEngine;

[Serializable]
public struct Node
{
    public float gCost;
    public float hCost;
    public float fCost;
    public bool traversable;
    public bool isEndNode;
    public bool isStartNode;
    public Vector3Int parent;
    public Vector3Int position;

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
