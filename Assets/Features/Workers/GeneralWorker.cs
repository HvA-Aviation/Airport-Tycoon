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
    public class GeneralWorker : Worker
    {
        [SerializeField] private NPCController _npcController;
        [SerializeField] private Grid _grid;
        [SerializeField] private float _workLoadSpeed;
        private Vector3Int _assignment;
        private UtilityData _data;

        private void Start()
        {
            // Register it to the task system by setting it available.
            GameManager.Instance.TaskManager.GeneralTaskSystem.SetAvailable(this);
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
                if (!CheckTaskExists(_assignment, onDone))
                    yield break;

                if (GameManager.Instance.QueueManager.QueueExists(_assignment) &&
                    GameManager.Instance.QueueManager.IsQueued(_assignment))
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
            _npcController.SetTarget(
                new Vector3Int(target.x, target.y, 0),
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