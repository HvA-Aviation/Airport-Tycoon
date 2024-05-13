using System;
using Implementation.Workers;
using UnityEngine;

namespace Implementation.Workers
{
    public class BuilderTaskSystem : MonoBehaviour
    {
        private TaskSystem<BuilderWorker> _taskSystem = new();

        public TaskSystem<BuilderWorker> GetTaskSystem()
        {
            return _taskSystem;
        }
    }
}