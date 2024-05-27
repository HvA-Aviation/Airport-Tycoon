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
    public class SecurityWorker : Worker
    {
        [SerializeField] private NPCController _npcController;
        [SerializeField] private Grid _grid;
        [SerializeField] private float _workLoadSpeed;
        [SerializeField] private int _checksBeforeShiftChange;
        private Vector3Int _assignment;

        private void Start()
        {
            // Register it to the task system by setting it available.
            GameManager.Instance.TaskManager.SecurityTaskSystem.SetAvailable(this);
        }

        public void Guard(Action onDone)
        {
            StartCoroutine(WorkOn(onDone));
        }

        private IEnumerator WorkOn(Action onDone)
        {
            int times = 0;
            float utilityWorkload = _grid.GetWorkLoad(_assignment);
            while (times < _checksBeforeShiftChange)
            {
                if (GameManager.Instance.QueueManager.QueueExists(_assignment) &&
                    GameManager.Instance.QueueManager.IsQueued(_assignment))
                {
                    if (GameManager.Instance.QueueManager.WorkOnQueue(_assignment, _workLoadSpeed, utilityWorkload))
                    {
                        GameManager.Instance.QueueManager.RemoveFromQueue(_assignment);
                        times++;
                    }
                }

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