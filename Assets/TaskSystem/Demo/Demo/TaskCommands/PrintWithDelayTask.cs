using System;


public class PrintWithDelayTask : TaskCommand<DebugWorker>
{
    private string message;

    public PrintWithDelayTask(string message)
    {
        this.message = message;
    }

    protected override void ExecuteInternal(DebugWorker worker, Action onTaskDone)
    {
        FunctionTimer.Create(() =>
        {
            worker.Print(message);
            onTaskDone();
        }, 5);
    }
}