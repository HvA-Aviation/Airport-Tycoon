using UnityEngine;

public class DebugWorker : Worker
{
    [SerializeField]
    private TaskSystemContainer _taskSystemContainer;

    private void Start()
    {
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