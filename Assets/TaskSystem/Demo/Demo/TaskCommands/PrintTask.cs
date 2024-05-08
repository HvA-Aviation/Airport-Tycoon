using System;

public class PrintTask : TaskCommand<DebugWorker>
{
    private string message;

    public PrintTask(string message)
    {
        this.message = message;
    }

    protected override void ExecuteInternal(DebugWorker worker, Action onTaskDone)
    {
        worker.Print(message);
        onTaskDone();
    }
}