using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes
{
    [CreateAssetMenu(fileName = "Building/Atlas", menuName = "Building/Atlas", order = 0)]
    public class BuildableAtlas : ScriptableObject
    {
        public static readonly int Empty = -1;
        public TileData[] Items;
        
        public TileEntry[] Tiles;
    }
    
    [System.Serializable]
    public class TileEntry
    {
        public TileType tileType;
        [SerializeReference]
        public BaseTile tile;
    }

    public enum TileType
    {
        Normal,
        Utility
    }

    [System.Serializable]
    public class BaseTile
    {
        public Tile Tile;
        public Color Color = Color.white;
        public float BuildLoad;
        public bool UnTraversable;
    }

    [System.Serializable]
    public class NormalTile : BaseTile
    {
    }

    [System.Serializable]
    public class UtilityTile : BaseTile
    {
        public string UtilityType;
        public int Workload;
    }
}