using System;

// Command containing task to print an error.

// Create a class deriving from TaskCommand and the worker it's aimed at.
public class PrintErrorTask : TaskCommand<DebugWorker>
{
    private string message;

    // Set task data.
    public PrintErrorTask(string message)
    {
        this.message = message;
    }

    // Set task instructions.
    protected override void ExecuteInternal(DebugWorker worker, Action onTaskDone)
    {
        worker.Print(message);
        onTaskDone();
    }
}