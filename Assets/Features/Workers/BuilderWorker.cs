using System;
using System.Collections;
using Features.Managers;
using Features.Workers.TaskCommands;
using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using UnityEngine;

namespace Features.Workers
{
    public class BuilderWorker : Worker
    {
        [SerializeField] private NPCController _npcController;
        [SerializeField] private float _workLoadSpeed;

        [Tooltip("Reset worker if it has been stuck for n amount of times"), Range(0, 10)]
        [SerializeField] int ResetToSpawnPointLimit = 1;
        int ResetToSpawnPointCurrent;
        
        protected TaskCommand<BuilderWorker> _task;
        
        public void SetTask(TaskCommand<BuilderWorker> taskCommand)
        {
            _task = taskCommand;
        }

        private void Start()
        {
            // Register it to the task system by setting it available.
            GameManager.Instance.TaskManager.BuilderTaskSystem.SetAvailable(this);
        }

        public void Build(Vector3Int target, Action onBuilt)
        {
            StartCoroutine(CheckBuild(target, onBuilt));
        }

        private IEnumerator CheckBuild(Vector3Int target, Action onBuilt)
        {
            while (!GameManager.Instance.GridManager.Grid.BuildTile(target, _workLoadSpeed))
            {
                yield return null;
            }

            onBuilt.Invoke();

            // Temporary solution to avoid errors when passengers dont have a utility to go to
            GameManager.Instance.EventManager.TriggerEvent(EventManager.EventId.OnMissingUtility);
        }

        public void MoveTo(Vector3Int target, Action onReachedPosition, Action onDone)
        {
            _npcController.SetTarget(
                new Vector3Int(target.x, target.y, 0),
                () => CheckTaskExists(target, onDone),
                onReachedPosition,
                () => StartCoroutine(TaskUnreachable(target)));
        }

        IEnumerator TaskUnreachable(Vector3Int target)
        {
            ResetToSpawnPointCurrent++;

            Debug.LogWarning("Worker could not find path to target, releasing task and adding it to task queue");
            GameManager.Instance.TaskManager.BuilderTaskSystem.AddTask(new BuildTask(target));

            yield return new WaitForSecondsRealtime(10f);

            if (ResetToSpawnPointCurrent > ResetToSpawnPointLimit)
                transform.position = GameManager.Instance.GridManager.GetPaxSpawnPoint();

            GameManager.Instance.TaskManager.BuilderTaskSystem.SetAvailable(this);
        }

        /// <summary>
        /// Checks if task is still needed
        /// </summary>
        private void CheckTaskExists(Vector3Int target, Action onDone)
        {
            if (GameManager.Instance.GridManager.Grid.IsEmpty(target))
                onDone.Invoke();
        }
        
        /// <summary>
        /// Stop all current routines and add the current task back to the task list
        /// </summary>
        public void Fire()
        {
            StopAllCoroutines();
            _npcController.StopAllCoroutines();
            if (_task != null)
                GameManager.Instance.TaskManager.BuilderTaskSystem.AddTask(_task);
        }
    }
}