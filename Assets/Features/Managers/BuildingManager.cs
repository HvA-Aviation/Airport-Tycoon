using System;
using System.Collections.Generic;
using System.Linq;
using Features.Building.Scripts.Datatypes;
using Features.EventManager;
using UnityEngine;

namespace Features.Managers
{
    public class BuildingManager : MonoBehaviour
    {
        [field: SerializeField] public BuildableObject CurrentBuildableObject { get; private set; }
        [field: SerializeField] public List<BuildingStatus> BuildingStatuses { get; private set; }

        /// <summary>
        /// Set other buildable
        /// </summary>
        /// <param name="buildableObject">Building that is going to be placed</param>
        public void ChangeSelectedBuildable(BuildableObject buildableObject)
        {
            CurrentBuildableObject = buildableObject;
            GameManager.Instance.EventManager.TriggerEvent(EventId.ChangeBrush);
        }

        public void UnlockBuilding(BuildableObject buildableObject)
        {
            int index = BuildingStatuses.FindIndex(x => x.BuildableObject == buildableObject);
            if (index != -1)
            {
                BuildingStatuses[index].IsUnlocked = true;
            }
            else
            {
                Debug.LogError(buildableObject.name + " not found in the Building Manager");
            }
        }
    }

    [Serializable]
    public class BuildingStatus
    {
        [field: SerializeField] public BuildableObject BuildableObject { get; private set; }
        [field: SerializeField] public bool IsUnlocked { get; set; }
        [field: SerializeField] public BuildingCategory Category { get; private set; }
    }
}