using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using Features.EventManager;
using UnityEngine;

namespace Features.Managers
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] public BuildableObject CurrentBuildableObject { get; private set; }
        [SerializeField] private List<BuildingStatus> _buildingStatuses;

        /// <summary>
        /// Set other buildable
        /// </summary>
        /// <param name="buildableObject">Building that is going to be placed</param>
        public void ChangeSelectedBuildable(BuildableObject buildableObject)
        {
            CurrentBuildableObject = buildableObject;
            GameManager.Instance.EventManager.TriggerEvent(EventId.ChangeBrush);
        }
    }

    [Serializable]
    public class BuildingStatus
    {
        [field: SerializeField] public BuildableObject BuildableObject { get; private set; }
        [field: SerializeField] public bool IsLocked { get; private set; }
    }
}