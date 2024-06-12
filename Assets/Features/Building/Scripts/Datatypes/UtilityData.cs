using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using UnityEngine;

namespace Features.Building.Scripts.Datatypes
{
    public struct UtilityData
    {
        public Vector3Int Position;
        public NPCController User;
        public float Progression;

        public UtilityData(Vector3Int position)
        {
            Position = position;
            User = null;
            Progression = 0;
        }
        
        public void AssignUser(NPCController passenger)
        {
            User = passenger;
            Progression = 0;
        }
    }
}