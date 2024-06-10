using System;
using Features.Managers;
using Implementation.TaskSystem;
using UnityEngine;

// Command containing task to print an log.

// Create a class deriving from TaskCommand and the worker it's aimed at.

namespace Features.Workers.TaskCommands
{
    public class OperateTask : TaskCommand<AssignableWorker>
    {
        private Vector3Int _targetPosition;

        // Set task data.
        public OperateTask(Vector3Int targetPosition)
        {
            _targetPosition = targetPosition;
        }

        // Set task instructions.
        protected override void ExecuteInternal(AssignableWorker worker, Action onTaskDone)
        {
            OperateTask task = new OperateTask(_targetPosition);
            
            worker.MoveTo(_targetPosition, () => {  worker.Station(() =>
            {
                worker.TaskManager().AddTask(task);
                onTaskDone?.Invoke();
            }); }, onTaskDone);
        }
    }
}