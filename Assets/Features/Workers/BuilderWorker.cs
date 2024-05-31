using System;
using System.Collections;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using UnityEngine;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Features.Workers
{
    public class BuilderWorker : Worker
    {
        [SerializeField] private NPCController _npcController;
        [SerializeField] private Grid _grid;
        [SerializeField] private float _workLoadSpeed;

        private void OnEnable()
        {
            _grid = FindObjectOfType<Grid>();
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
            while (!_grid.BuildTile(target, _workLoadSpeed))
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
        /// Checks if task is still needed
        /// </summary>
        private void CheckTaskExists(Vector3Int target, Action onDone)
        {
            if (_grid.Get(target) == -1) onDone.Invoke();
        }
    }
}