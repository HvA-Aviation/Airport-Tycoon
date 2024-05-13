using System;

// Command containing task to print an log.

// Create a class deriving from TaskCommand and the worker it's aimed at.

namespace Implementation.TaskSystem.Demo.TaskCommands
{
    public class PrintTask : TaskCommand<DebugWorker>
    {
        private string message;

        // Set task data.
        public PrintTask(string message)
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
}