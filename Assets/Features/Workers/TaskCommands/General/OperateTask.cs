using System;
using Features.Managers;
using Implementation.TaskSystem;
using UnityEngine;

namespace Features.Workers.TaskCommands
{
    public class OperateTask : TaskCommand<AssignableWorker>
    {
        private Vector3Int _targetPosition;

        /// <summary>
        /// Sets the target position of the task
        /// </summary>
        /// <param name="targetPosition">The position of the task</param>
        public OperateTask(Vector3Int targetPosition)
        {
            _targetPosition = targetPosition;
        }

        /// <summary>
        /// Create a task that makes a assignable worker move to the target and let it work on it
        /// </summary>
        /// <param name="worker">Assignable worker like guard</param>
        /// <param name="onTaskDone">What will happen if the task is unavailable or finished</param>
        protected override void ExecuteInternal(AssignableWorker worker, Action onTaskDone)
        {
            OperateTask task = new OperateTask(_targetPosition);
            worker.SetTask(task);
            
            worker.MoveTo(_targetPosition, () => {  worker.Station(() =>
            {
                worker.TaskManager().AddTask(task);
                onTaskDone?.Invoke();
            }); }, onTaskDone);
        }
    }
}