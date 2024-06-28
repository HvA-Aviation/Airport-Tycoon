using System;
using Implementation.TaskSystem;
using UnityEngine;

// Command containing task to print an log.

// Create a class deriving from TaskCommand and the worker it's aimed at.

namespace Features.Workers.TaskCommands
{
    public class BuildTask : TaskCommand<BuilderWorker>
    {
        private Vector3Int _targetPosition;

        // Set task data.
        public BuildTask(Vector3Int targetPosition)
        {
            _targetPosition = targetPosition;
        }

        // Set task instructions.
        protected override void ExecuteInternal(BuilderWorker worker, Action onTaskDone)
        {
            BuildTask task = new BuildTask(_targetPosition);
            worker.SetTask(task);
            
            worker.MoveTo(_targetPosition, () => { worker.Build(_targetPosition, onTaskDone); }, onTaskDone);
        }
    }
}