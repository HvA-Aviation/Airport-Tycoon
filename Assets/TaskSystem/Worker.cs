using UnityEngine;

public abstract class Worker : MonoBehaviour
{
    public enum WorkerState
    {
        Busy,
        Free
    }

    public WorkerState State { get; private set; }
    public void SetState(WorkerState state) {  State = state; }
}