using System;

public class PrintErrorTask : TaskCommand<DebugWorker>
{
    private string message;

    public PrintErrorTask(string message)
    {
        this.message = message;
    }

    protected override void ExecuteInternal(DebugWorker worker, Action onTaskDone)
    {
        worker.PrintError(message);
        onTaskDone();
    }
}