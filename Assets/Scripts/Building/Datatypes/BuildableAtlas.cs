using UnityEngine;

namespace Building.Datatypes
{
    [CreateAssetMenu(fileName = "Building/Atlas", menuName = "Building/Atlas", order = 0)]
    public class BuildableAtlas : ScriptableObject
    {
        public TileData[] Items;
    }
}