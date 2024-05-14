namespace Implementation.TaskSystem
{
    public struct WorkerTask<WorkerType> where WorkerType : Worker
    {
        public WorkerType Worker;
        public TaskCommand<WorkerType> Task;
        
        public WorkerTask(WorkerType worker, TaskCommand<WorkerType> task)
        {
            Worker = worker;
            Task = task;
        }
    }
}