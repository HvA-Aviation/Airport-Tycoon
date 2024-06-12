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

        private List<BuildingStatus> _unlocked = new List<BuildingStatus>();

        private void Start()
        {
            foreach (BuildingStatus status in BuildingStatuses)
            {
                if (status.IsUnlocked)
                    _unlocked.Add(status);
            }
        }

        /// <summary>
        /// Set other buildable
        /// </summary>
        /// <param name="buildableObject">Building that is going to be placed</param>
        public void ChangeSelectedBuildable(int index)
        {
            CurrentBuildableObject = _unlocked[index].BuildableObject;
            GameManager.Instance.EventManager.TriggerEvent(EventId.OnChangeBrush);
        }

        public void UnlockBuilding(BuildableObject buildableObject)
        {
            int index = BuildingStatuses.FindIndex(x => x.BuildableObject == buildableObject);
            if (index != BuildableAtlas.Empty)
            {
                BuildingStatuses[index].IsUnlocked = true;
                _unlocked.Insert(index, BuildingStatuses[index]);
                GameManager.Instance.EventManager.TriggerEvent(EventId.OnUnlockBuilding);
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
