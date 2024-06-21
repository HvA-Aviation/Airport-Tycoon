using Features.Managers;
using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Features.Workers
{
    public class BuilderWorker : Worker
    {
        [SerializeField] private NPCController _npcController;
        [SerializeField] private float _defaultWorkLoadSpeed;

        public float WorkLoadSpeed { get; private set; }

        private void Start()
        {
            // Register it to the task system by setting it available.
            GameManager.Instance.TaskManager.BuilderTaskSystem.SetAvailable(this);
            WorkLoadSpeed = _defaultWorkLoadSpeed;
        }

        public void Build(Vector3Int target, Action onBuilt)
        {
            StartCoroutine(CheckBuild(target, onBuilt));
        }

        private IEnumerator CheckBuild(Vector3Int target, Action onBuilt)
        {
            while (!GameManager.Instance.GridManager.Grid.BuildTile(target, WorkLoadSpeed))
            {
                yield return null;
            }

            onBuilt.Invoke();
        }

        public void MoveTo(Vector3Int target, Action onReachedPosition, Action onDone)
        {
            _npcController.SetTarget(
                new Vector3Int(target.x, target.y, 0),
                () => CheckTaskExists(target, onDone),
                onReachedPosition);
        }

        /// <summary>
        /// Call this function to change the workSpeed
        /// </summary>
        /// <param name="mulitplier">The amount the workspeed will change from the default speed</param>
        public void SetWorkSpeed(float mulitplier) => WorkLoadSpeed = _defaultWorkLoadSpeed * mulitplier;

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