using System;

namespace Features.Building.Scripts.Datatypes.TileData
{
    [Serializable]
    public class UtilityTile : BaseTile
    {
        public UtilityType UtilityType;
        public int Workload;
    }
}