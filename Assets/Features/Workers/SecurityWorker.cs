﻿using System;
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
    public class SecurityWorker : Worker
    {
        [SerializeField] private NPCController _npcController;
        [SerializeField] private Grid _grid;
        [SerializeField] private float _workLoadSpeed;
        private Vector3Int _assignment;
        private UtilityData _data;
        
        private void Start()
        {
            // Register it to the task system by setting it available.
            GameManager.Instance.TaskManager.SecurityTaskSystem.SetAvailable(this);
        }
        
        public void Guard(Action onDone)
        {
            _data = _grid.GetUtilities(UtilityType.Security).First(x => x.Position == _assignment);
            StartCoroutine(WorkOn(10f, onDone));
        }

        private IEnumerator WorkOn(float time, Action onDone)
        {
            while (time > 0)
            {
                if (_grid.WorkOnUtility(UtilityType.Security, _assignment, _workLoadSpeed))
                {
                    
                }
                
                time -= Time.deltaTime;
                yield return null;
            }
            
            onDone?.Invoke();
        }

        private IEnumerator Wait(float time, Action onDone)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            
            onDone?.Invoke();
        }

        public void MoveTo(Vector3Int target, Action onReachedPosition, Action onDone)
        {
            _assignment = target;
            _npcController.SetTarget(
                new Vector3Int(target.x, target.y, 0),
                () => CheckTaskExists(target, onDone),
                onReachedPosition);
        }
        
        /// <summary>
        /// Checks if task is still needed
        /// </summary>
        private void CheckTaskExists(Vector3Int target, Action onDone)
        {
            if (_grid.Get(target) == -1) onDone.Invoke();
        }
    }
}