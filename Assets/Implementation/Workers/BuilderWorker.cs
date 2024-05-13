using System;
using System.Collections;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using UnityEngine;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Implementation.Workers
{
    public class BuilderWorker : Worker
    {
        [SerializeField] private NPCController _npcController;
        [SerializeField] private Grid _grid;

        private void Start()
        {
            // Register it to the task system by setting it available.
            GameManager.Instance.TaskManager.BuilderTaskSystem.GetTaskSystem().SetAvailable(this);
        }

        public void Build(Vector3Int target, Action onBuilt)
        {
            StartCoroutine(CheckBuild(target, onBuilt));
        }

        private IEnumerator CheckBuild(Vector3Int target, Action onBuilt)
        {
            while (!_grid.BuildTile(target, 2f))
            {
                yield return null;
            }

            onBuilt.Invoke();
        }

        public void MoveTo(Vector3Int target, Action onReachedPosition, Action onDone)
        {
            StartCoroutine(CheckReached(target, onReachedPosition, onDone));
        }

        private IEnumerator CheckReached(Vector3Int target, Action onReachedPosition, Action onDone)
        {
            _npcController.SetTarget(new Vector3Int(target.x, target.y, 0));
            while (!_npcController.PathCompleted)
            {
                //Checks if task is still needed
                if (_grid.Get(target) == -1)
                {
                    onDone.Invoke();
                    _npcController.StopAllCoroutines();
                    yield break;
                }

                yield return new WaitForSeconds(.5f);
            }

            onReachedPosition.Invoke();
        }
    }
}