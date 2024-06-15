using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes
{
    [Serializable]
    public class TileData
    {
        [field: SerializeField] public Tile Tile { get; private set; }
        public Color Color = Color.white;

        public float WorkLoad;
        public bool UnTraversable = false;

        [Header("Utility Settings")]
        public UtilityType UtilityType;
        public float UtilityLoad;

        [Header("Behavior Settings")]
        public BehaviourType BehaviorType;
    }
}