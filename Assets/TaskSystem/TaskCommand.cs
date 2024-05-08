using System;

public abstract class TaskCommand<WorkerType> 
    where WorkerType : Worker
{
    public void Execute(WorkerType worker, Action onTaskDone)
    {
        Prepare(worker);
        ExecuteInternal(worker, onTaskDone);
    }
    
    protected abstract void ExecuteInternal(WorkerType worker, Action onTaskDone);
    
    private void Prepare(WorkerType worker) { worker.SetState(Worker.WorkerState.Busy); }
}