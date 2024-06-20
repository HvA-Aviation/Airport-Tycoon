using System;
using Features.Building.Scripts.Datatypes.TileData;
using UnityEngine;

namespace Features.Building.Scripts.Datatypes
{
    [Serializable]
    public class TileEntry
    {
        public TileType TileType;
        [SerializeReference]
        public BaseTile TileData;
    }
}