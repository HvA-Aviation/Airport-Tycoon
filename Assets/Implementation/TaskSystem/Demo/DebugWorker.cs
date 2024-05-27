using UnityEngine;

// Create a worker and derive it from the Worker class.
namespace Implementation.TaskSystem.Demo
{
    public class DebugWorker : Worker
    {
        [SerializeField]
        private TaskSystemContainer _taskSystemContainer;

        private void Start()
        {
            // Register it to the task system by setting it available.
            _taskSystemContainer.GetTaskSystem().SetAvailable(this);
        }

        public void Print(string message)
        {
            Debug.Log(message);
        }

        public void PrintError(string message)
        {
            Debug.LogError(message);
        }
    }
}