using System;
using Features.Managers;
using Implementation.TaskSystem;
using UnityEngine;

// Command containing task to print an log.

// Create a class deriving from TaskCommand and the worker it's aimed at.

namespace Features.Workers.TaskCommands
{
    public class GeneralOperateTask : TaskCommand<GeneralWorker>
    {
        private Vector3Int _targetPosition;

        // Set task data.
        public GeneralOperateTask(Vector3Int targetPosition)
        {
            _targetPosition = targetPosition;
        }

        // Set task instructions.
        protected override void ExecuteInternal(GeneralWorker worker, Action onTaskDone)
        {
            GeneralOperateTask task = new GeneralOperateTask(_targetPosition);
            
            worker.MoveTo(_targetPosition, () => {  worker.Guard(() =>
            {
                GameManager.Instance.TaskManager.GeneralTaskSystem.AddTask(task);
                onTaskDone?.Invoke();
            }); }, onTaskDone);
        }
    }
}