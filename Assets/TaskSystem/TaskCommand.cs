using System;

/// <summary>
/// Wrapper class for easier use of the Task System's Command Pattern.
/// </summary>
/// <typeparam name="WorkerType">Type of worker that should be able to execute this task.</typeparam>
public abstract class TaskCommand<WorkerType> 
    where WorkerType : Worker
{
    /// <summary>
    /// Execute the task.
    /// </summary>
    /// <param name="worker">Worker assigned to this task.</param>
    /// <param name="onTaskDone">Callback when the task is done.</param>
    public void Execute(WorkerType worker, Action onTaskDone)
    {
        Prepare(worker);
        ExecuteInternal(worker, onTaskDone);
    }
    
    /// <summary>
    /// Task instructions.
    /// </summary>
    /// <param name="worker">Worker assigned to this task.</param>
    /// <param name="onTaskDone">Callback when the task is done.</param>
    protected abstract void ExecuteInternal(WorkerType worker, Action onTaskDone);
    
    /// <summary>
    /// Prepares the worker for the task.
    /// </summary>
    /// <param name="worker">Worker assigned to the task.</param>
    private void Prepare(WorkerType worker) { worker.SetState(Worker.WorkerState.Busy); }
}