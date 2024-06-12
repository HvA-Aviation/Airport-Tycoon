using UnityEngine;

namespace Features.Building.Scripts.Datatypes
{
    [CreateAssetMenu(fileName = "Building/Atlas", menuName = "Building/Atlas", order = 0)]
    public class BuildableAtlas : ScriptableObject
    {
        public static readonly int Empty = -1;
        public TileData[] Items;
    }
}