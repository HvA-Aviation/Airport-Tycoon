using Features.Managers;
using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Features.Workers
{
    public abstract class AssignableWorker : Worker
    {
        [SerializeField] protected NPCController _npcController;
        [SerializeField] protected float _defaultWorkLoadSpeed;
        [SerializeField] protected int _assignmentsShift;
        protected Vector3Int _assignment;
        protected Vector3Int _targetPosition;
        protected TaskCommand<AssignableWorker> _task;

        private float _currentWorkLoadSpeed;

        protected virtual void Start()
        {
            // Register it to the task system by setting it available.
            TaskManager().SetAvailable(this);
            _currentWorkLoadSpeed = _defaultWorkLoadSpeed;
        }

        /// <summary>
        /// Gets the direction of the utility so the worker will stand behind it
        /// </summary>
        /// <param name="target">Position of the utility</param>
        /// <returns>The position behind the utility</returns>
        protected Vector3Int GetRotationAddition(Vector3Int target)
        {
            int rotation = GameManager.Instance.GridManager.Grid.GetRotation(target);
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

            return new Vector3Int(target.x, target.y, 0) + rotationVector;
        }

        /// <summary>
        /// call this function to multiply the currentworkspeed with the multiplier
        /// </summary>
        /// <param name="multiplier">The amount the workspeed will change from the default speed</param>
        public void SetWorkSpeed(float multiplier) => _currentWorkLoadSpeed = _defaultWorkLoadSpeed * multiplier;

        public abstract TaskSystem<AssignableWorker> TaskManager();

        public void SetTask(TaskCommand<AssignableWorker> taskCommand)
        {
            _task = taskCommand;
        }

        /// <summary>
        /// Starts a routine so the worker will work on the utility
        /// </summary>
        /// <param name="onDone">Sets the worker back in the task queue</param>
        public void Station(Action onDone)
        {
            StartCoroutine(WorkOn(onDone));
        }

        /// <summary>
        /// Works on the queue till the assigment shift has been met
        /// </summary>
        /// <param name="onDone">Sets the worker back in the task queue</param>
        protected IEnumerator WorkOn(Action onDone)
        {
            float workload = GameManager.Instance.GridManager.Grid.GetUtilityWorkLoad(_assignment);

            int times = 0;
            while (times < _assignmentsShift)
            {
                if (!CheckTaskExists(_assignment, onDone))
                    yield break;

                if (GameManager.Instance.QueueManager.HasQueuers(_assignment))
                {
                    if (GameManager.Instance.QueueManager.WorkOnQueue(_assignment, _currentWorkLoadSpeed, workload))
                    {
                        GameManager.Instance.QueueManager.RemoveFromQueue(_assignment);
                        times++;
                    }
                }

                yield return null;
            }

            _task = null;
            onDone?.Invoke();
        }

        /// <summary>
        /// Move to grid location
        /// </summary>
        /// <param name="target">Target location</param>
        /// <param name="onReachedPosition">What to do on the target location</param>
        /// <param name="onDone">What to do when all is done</param>
        public void MoveTo(Vector3Int target, Action onReachedPosition, Action onDone)
        {
            //Checks if utility still exitst
            if (GameManager.Instance.GridManager.Grid.IsWorkDone(target))
            {
                _task = null;
                onDone.Invoke();
                return;
            }

            _assignment = target;
            _targetPosition = GetRotationAddition(target);

            _npcController.SetTarget(
                _targetPosition,
                () => CheckTaskExists(target, onDone),
                onReachedPosition);
        }

        /// <summary>
        /// Checks if task is still needed
        /// </summary>
        protected bool CheckTaskExists(Vector3Int target, Action onDone)
        {
            if (GameManager.Instance.GridManager.Grid.IsEmpty(target))
            {
                _task = null;
                onDone.Invoke();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Stop all current routines and add the current task back to the task list
        /// </summary>
        public void Fire()
        {
            StopAllCoroutines();
            _npcController.StopAllCoroutines();
            if (_task != null)
                TaskManager().AddTask(_task);
        }
    }
}