using System.Collections.Generic;
using UnityEngine;

namespace Features.Building.Scripts.Datatypes
{
    [CreateAssetMenu(fileName = "BuildItem", menuName = "Building/BuildItem", order = 0)]
    public class BuildableObject : ScriptableObject
    {
        public string Name;
        public BrushType BrushType;
        public List<SubBuildItem> BuildItems;
    }
}