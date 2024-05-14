using System;
using Features.EventManager;
using Features.Managers;
using Implementation.TaskSystem;
using UnityEngine;

namespace Implementation.Workers
{
    public class BuilderTaskSystem : MonoBehaviour
    {
        private TaskSystem<BuilderWorker> _taskSystem = new();

        private void Start()
        {
            //Make sure to check for available tasks that were previously unavailable
            GameManager.Instance.EventManager.Subscribe(EventId.UpdateMap, (EventArgs) => _taskSystem.TryAssignTask());
        }

        public TaskSystem<BuilderWorker> GetTaskSystem()
        {
            return _taskSystem;
        }
    }
}