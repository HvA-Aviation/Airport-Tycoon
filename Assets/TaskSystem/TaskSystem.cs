using System.Collections.Generic;

public class TaskSystem<WorkerType> 
    where WorkerType: Worker 
{
    private Queue<WorkerType> _availableWorkers = new Queue<WorkerType>();
    private Queue<TaskCommand<WorkerType>> _queue = new Queue<TaskCommand<WorkerType>>();

    public void SetAvailable(WorkerType workerType) 
    {  
        _availableWorkers.Enqueue(workerType);
        TryAssignTask();
    }

    public void AddTask(TaskCommand<WorkerType> task) 
    { 
        _queue.Enqueue(task);
        TryAssignTask();
    }

    private void TryAssignTask()
    {
        if(_availableWorkers.Count > 0 && _queue.Count > 0) 
        { 
            WorkerType worker = _availableWorkers.Dequeue();
            TaskCommand<WorkerType> task = _queue.Dequeue();
            task.Execute(worker, () => {
                worker.SetState(Worker.WorkerState.Free);
                SetAvailable(worker); 
            });
        }
    }
}


