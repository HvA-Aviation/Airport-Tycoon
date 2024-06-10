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
        public override TaskSystem<AssignableWorker> TaskManager()
        {
            return GameManager.Instance.TaskManager.SecurityTaskSystem;
        }
    }
}