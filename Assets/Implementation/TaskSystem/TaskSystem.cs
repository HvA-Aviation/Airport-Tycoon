using System.Collections.Generic;
using System.Linq;

namespace Implementation.TaskSystem
{
    /// <summary>
    /// Tasksystem for the given WorkerType.
    /// </summary>
    /// <typeparam name="WorkerType">Type of worker that should be able to execute the tasks.</typeparam>
    public class TaskSystem<WorkerType>
        where WorkerType : Worker
    {
        /// <summary>
        /// Collection of available workers in the system.
        /// </summary>
        private Queue<WorkerType> _availableWorkers = new Queue<WorkerType>();

        /// <summary>
        /// Collection of tasks in the system.
        /// </summary>
        private Queue<TaskCommand<WorkerType>> _queue = new Queue<TaskCommand<WorkerType>>();

        /// <summary>
        /// Allows the worker to pick up tasks.
        /// </summary>
        /// <param name="workerType">Worker that will be available.</param>
        public void SetAvailable(WorkerType workerType)
        {
            _availableWorkers.Enqueue(workerType);
            TryAssignTask();
        }

        /// <summary>
        /// Adds a task to the system.
        /// </summary>
        /// <param name="task">Task that should be added to the list.</param>
        public void AddTask(TaskCommand<WorkerType> task)
        {
            _queue.Enqueue(task);
            TryAssignTask();
        }

        /// <summary>
        /// Checks whether there is a task and an available worker and assigns a worker to a task if possible.
        /// </summary>
        private void TryAssignTask()
        {
            if (_availableWorkers.Count > 0 && _queue.Count > 0)
            {
                WorkerTask<WorkerType> workerTask = FindAvailableTask(_queue);
                if (workerTask == default)
                    return;

                _availableWorkers = new Queue<WorkerType>(_availableWorkers.Where(x => x != workerTask.Worker));
                _queue = new Queue<TaskCommand<WorkerType>>(_queue.Where(x => x != workerTask.Task));

                workerTask.Task.Execute(workerTask.Worker, () =>
                {
                    workerTask.Worker.SetState(Worker.WorkerState.Free);
                    SetAvailable(workerTask.Worker);
                });
            }
        }

        private WorkerTask<WorkerType> FindAvailableTask(Queue<TaskCommand<WorkerType>> queue)
        {
            foreach (var worker in _availableWorkers)
            {
                foreach (var task in queue)
                {
                    if (task.IsAvailable(worker))
                        return new WorkerTask<WorkerType>(worker, task);
                }
            }

            return default;
        }
    }
}