using UnityEngine;

public class TaskSystemContainer : MonoBehaviour
{
    [SerializeField]
    private TaskSystem<DebugWorker> _taskSystem = new();

    public TaskSystem<DebugWorker> GetTaskSystem() { return _taskSystem; }
}