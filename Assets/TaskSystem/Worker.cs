using UnityEngine;

/// <summary>
/// Base class for a worker.
/// </summary>
public abstract class Worker : MonoBehaviour
{
    /// <summary>
    /// Possible states of the worker.
    /// </summary>
    public enum WorkerState
    {
        Busy,
        Free
    }

    /// <summary>
    /// Current state of the worker.
    /// </summary>
    public WorkerState State { get; private set; }

    /// <summary>
    /// Sets the state of the worker.
    /// </summary>
    /// <param name="state">State of the worker.</param>
    public void SetState(WorkerState state) {  State = state; }
}