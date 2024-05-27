using System.Collections;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using Features.Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "Unlockable", menuName = "Unlockable/BuildingUnlock", order = 0)]
public class BuildingUnlock : Unlockable
{
    [field: SerializeField] public List<BuildableObject> BuildableObjects { get; private set; }

    public override void Execute()
    {
        foreach (BuildableObject buildableObject in BuildableObjects)
        {
            GameManager.Instance.BuildingManager.UnlockBuilding(buildableObject);
        }
    }
}