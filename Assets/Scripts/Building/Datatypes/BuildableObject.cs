using UnityEngine;
using UnityEngine.Tilemaps;

namespace Building.Datatypes
{
    [CreateAssetMenu(fileName = "Building/BuildItem", menuName = "Building/BuildItem", order = 0)]
    public class BuildableObject : ScriptableObject
    {
        public string Name;
        public Tile Sprite;
        public Vector2Int Size;
    }
}