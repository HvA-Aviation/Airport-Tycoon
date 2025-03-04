using System;
using UnityEngine;

namespace Implementation.Pathfinding.Scripts
{
    [Serializable]
    public struct Node
    {
        public float gCost;
        public float hCost;
        public float fCost { get { return gCost + hCost; } }
        public bool untraversable;
        public bool isEndNode;
        public bool isStartNode;
        public Vector3Int parent;
        public Vector3Int position;
    }
}
