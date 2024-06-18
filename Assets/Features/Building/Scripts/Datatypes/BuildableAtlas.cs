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

        /// <summary>
        /// Get the default information of a tile by atlas index
        /// </summary>
        /// <param name="index">Index of the tile in this atlas</param>
        /// <returns>BaseTile information</returns>
        public BaseTile GetTileData(int index)
        {
            return GetTileData<BaseTile>(index);
        }
        
        /// <summary>
        /// Get the specific type information of a tile by atlas index
        /// </summary>
        /// <param name="index">Index of the tile in this atlas</param>
        /// <typeparam name="T">Tile type that is inherited from BaseTile</typeparam>
        /// <returns>Specific type information</returns>
        public T GetTileData<T>(int index) where T: BaseTile
        {
            return (T)Tiles[index].TileData;
        }
        
        /// <summary>
        /// Checks if a tile is a specific type by atlas index
        /// </summary>
        /// <param name="index">Index of the tile in this atlas</param>
        /// <typeparam name="T">Tile type that is inherited from BaseTile</typeparam>
        /// <returns>If the tile is a specific type</returns>
        public bool TileIsType<T>(int index) where T: BaseTile
        {
            return GetTileData(index) is T;
        }

        /// <summary>
        /// Get the default information of a tile by tile
        /// </summary>
        /// <param name="tile">Tile that is used in the atlas</param>
        /// <returns>BaseTile information</returns>
        public BaseTile GetTileData(Tile tile)
        {
            return GetTileData<BaseTile>(tile);
        }

        /// <summary>
        /// Checks if a tile is a specific type by tile
        /// </summary>
        /// <param name="tile">Tile that is used in the atlas</param>
        /// <typeparam name="T">Tile type that is inherited from BaseTile</typeparam>
        /// <returns>If the tile is a specific type</returns>
        public bool TileIsType<T>(Tile tile) where T: BaseTile
        {
            return GetTileData(tile) is T;
        }

        /// <summary>
        /// Get the specific type information of a tile by tile
        /// </summary>
        /// <param name="tile">Tile that is used in the atlas</param>
        /// <typeparam name="T">Tile type that is inherited from BaseTile</typeparam>
        /// <returns>Specific type information</returns>
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

        /// <summary>
        /// Gets the tile atlas index
        /// </summary>
        /// <param name="tile">Tile that is used in the atlas</param>
        /// <returns>Tile atlas index</returns>
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
    
    [Serializable]
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

    [Serializable]
    public class BaseTile
    {
        public Tile Tile;
        public Color Color = Color.white;
        public float BuildLoad;
        public bool UnTraversable;
    }

    [Serializable]
    public class NormalTile : BaseTile
    {
    }

    [Serializable]
    public class UtilityTile : BaseTile
    {
        public UtilityType UtilityType;
        public int Workload;
    }
}