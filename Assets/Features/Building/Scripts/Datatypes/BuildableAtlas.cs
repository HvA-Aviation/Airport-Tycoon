using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes
{
    [CreateAssetMenu(fileName = "Building/Atlas", menuName = "Building/Atlas", order = 0)]
    public class BuildableAtlas : ScriptableObject
    {
        public static readonly int Empty = -1;
        //public TileData[] Items;
        
        [SerializeField] public TileEntry[] Tiles;

        public BaseTile GetTileData(int index)
        {
            return GetTileData<BaseTile>(index);
        }
        
        public T GetTileData<T>(int index) where T: BaseTile
        {
            return (T)Tiles[index].TileData;
        }
        
        public bool TileIsType<T>(int index) where T: BaseTile
        {
            return GetTileData(index) is T;
        }

        public BaseTile GetTileData(Tile tile)
        {
            return GetTileData<BaseTile>(tile);
        }

        public bool TileIsType<T>(Tile tile) where T: BaseTile
        {
            return GetTileData(tile) is T;
        }

        public T GetTileData<T>(Tile tile) where T: BaseTile
        {
            T tileData = GetTileData<T>(GetTileDataIndex(tile));
            
            if (tileData == default)
            {
                Debug.LogError("Tile \"" + tile.name + "\" not found in Atlas");
                return default;
            }
            
            return tileData;
        }

        public int GetTileDataIndex(Tile tile)
        {
            int tileData = Array.FindIndex(Tiles, x => x.TileData.Tile == tile);
            
            if (tileData < 0)
            {
                Debug.LogError("Tile \"" + tile.name + "\" not found in Atlas");
                return default;
            }
            
            return tileData;
        }
    }
    
    [System.Serializable]
    public class TileEntry
    {
        public TileType TileType;
        [SerializeReference]
        public BaseTile TileData;
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
        public UtilityType UtilityType;
        public int Workload;
    }
}