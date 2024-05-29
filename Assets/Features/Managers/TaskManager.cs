using Features.Workers;
using Implementation.TaskSystem;
using UnityEngine;

namespace Features.Managers
{
    public class TaskManager : MonoBehaviour
    {
        public TaskSystem<BuilderWorker> BuilderTaskSystem { get; } = new();
        public TaskSystem<SecurityWorker> SecurityTaskSystem { get; } = new();
        public TaskSystem<GeneralWorker> GeneralTaskSystem { get; } = new();
    }
}