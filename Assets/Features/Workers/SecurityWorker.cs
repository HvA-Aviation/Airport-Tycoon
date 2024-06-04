using System;
using System.Collections;
using System.Linq;
using Features.Building.Scripts.Datatypes;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using UnityEngine;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Features.Workers
{
    public class SecurityWorker : AssignableWorker
    {
        private void Start()
        {
            // Register it to the task system by setting it available.
            GameManager.Instance.TaskManager.SecurityTaskSystem.SetAvailable(this);
        }
    }
}