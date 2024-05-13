using System;

// Command containing task to print a log after x amount of seconds.

// Create a class deriving from TaskCommand and the worker it's aimed at.
public class PrintWithDelayTask : TaskCommand<DebugWorker>
{
    private string message;
    private float delay;

    // Set task data.
    public PrintWithDelayTask(string message, float delay)
    {
        this.message = message;
        this.delay = delay;
    }

    // Set task instructions.
    protected override void ExecuteInternal(DebugWorker worker, Action onTaskDone)
    {
        FunctionTimer.Create(() =>
        {
            worker.Print(message);
            onTaskDone();
        }, delay);
    }
}