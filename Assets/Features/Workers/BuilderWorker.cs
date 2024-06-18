using System;
using System.Collections;
using Features.Building.Scripts.Datatypes;
using Features.Managers;
using Features.Workers.TaskCommands;
using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using UnityEngine;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Features.Workers
{
    public class BuilderWorker : Worker
    {
        [SerializeField] private NPCController _npcController;
        [SerializeField] private float _workLoadSpeed;

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
            Debug.LogWarning("Worker could not find path to target, releasing task and adding it to task queue");
            GameManager.Instance.TaskManager.BuilderTaskSystem.AddTask(new BuildTask(target));
            yield return new WaitForSecondsRealtime(10f);
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
    }
}