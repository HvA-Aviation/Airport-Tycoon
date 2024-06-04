using Features.Workers;
using Implementation.TaskSystem;
using UnityEngine;

namespace Features.Managers
{
    public class TaskManager : MonoBehaviour
    {
        public TaskSystem<BuilderWorker> BuilderTaskSystem { get; } = new();
        public TaskSystem<AssignableWorker> SecurityTaskSystem { get; } = new();
        public TaskSystem<AssignableWorker> GeneralTaskSystem { get; } = new();
    }
}