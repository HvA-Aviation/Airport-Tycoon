using System;
using System.Collections;
using UnityEngine;


public class PrintWithDelayTask : TaskCommand<DebugWorker>
{
    private string message;

    public PrintWithDelayTask(string message)
    {
        this.message = message;
    }

    protected override void ExecuteInternal(DebugWorker worker, Action onTaskDone)
    {
        // Start coroutine
    }

    private IEnumerator RunWithDelay(DebugWorker worker, Action onTaskDone)
    {
        yield return new WaitForSeconds(4);
        worker.Print(message);
        onTaskDone();
    }
}