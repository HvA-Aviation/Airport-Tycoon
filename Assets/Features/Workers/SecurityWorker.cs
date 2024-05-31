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
        private Vector3Int _assignment;
        private Vector3Int _targetPosition;

        private void OnEnable()
        {
            _grid = FindObjectOfType<Grid>();
        }
        
        private void Start()
        {
            // Register it to the task system by setting it available.
            GameManager.Instance.TaskManager.SecurityTaskSystem.SetAvailable(this);
        }

        public void Guard(Action onDone)
        {
            StartCoroutine(WorkOn(10f, onDone));
        }

        private IEnumerator WorkOn(float time, Action onDone)
        {
            int times = 0;
            while (times < 10)
            {
                if (GameManager.Instance.QueueManager.HasQueuers(_assignment))
                {
                    if (GameManager.Instance.QueueManager.WorkOnQueue(_assignment, _workLoadSpeed))
                    {
                        GameManager.Instance.QueueManager.RemoveFromQueue(_assignment);
                        times++;
                    }
                }

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
            
            int rotation = _grid.GetRotation(target);
            Vector3Int rotationVector = Vector3Int.down;

            switch (rotation)
            {
                case 0:
                    rotationVector = Vector3Int.up;
                    break;
                case 1:
                    rotationVector = Vector3Int.right;
                    break;
                case 2:
                    rotationVector = Vector3Int.down;
                    break;
                case 3:
                    rotationVector = Vector3Int.left;
                    break;
            }

            _targetPosition = new Vector3Int(target.x, target.y, 0) + rotationVector;
            
            _npcController.SetTarget(
                _targetPosition,
                () => CheckTaskExists(target, onDone),
                onReachedPosition);
        }

        /// <summary>
        /// Checks if task is still needed
        /// </summary>
        private bool CheckTaskExists(Vector3Int target, Action onDone)
        {
            if (_grid.Get(target) == -1)
            {
                onDone.Invoke();
                return false;
            }

            return true;
        }
    }
}