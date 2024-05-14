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
        public void TryAssignTask()
        {
            if (_availableWorkers.Count > 0 && _queue.Count > 0)
            {
                WorkerTask<WorkerType> workerTask = FilterAndFindAvailable();
                if (!workerTask.Worker)
                    return;

                workerTask.Task.Execute(workerTask.Worker, () =>
                {
                    workerTask.Worker.SetState(Worker.WorkerState.Free);
                    SetAvailable(workerTask.Worker);
                });
            }
        }

        /// <summary>
        /// Checks if the tasks are available for the workers if not they are put at the back of the queue
        /// </summary>
        /// <returns>A struct with the available worker and task</returns>
        private WorkerTask<WorkerType> FilterAndFindAvailable()
        {
            for (int i = 0; i < _availableWorkers.Count; i++)
            {
                var worker = _availableWorkers.Dequeue();

                for (int j = 0; j < _queue.Count; j++)
                {
                    var task = _queue.Dequeue();

                    if (!task.IsAvailable(worker))
                        _queue.Enqueue(task);
                    else
                    {
                        return new WorkerTask<WorkerType>(worker, task);
                    }
                }

                _availableWorkers.Enqueue(worker);
            }

            return default;
        }
    }
}