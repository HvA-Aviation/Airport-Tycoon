using UnityEngine;

// Add tasks to the tasksystem through the Command Pattern.
// https://refactoring.guru/design-patterns/command

public class Demo : MonoBehaviour
{
    [SerializeField]
    private TaskSystemContainer _taskSystemContainer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
            _taskSystemContainer.GetTaskSystem().AddTask(new PrintTask("Oh hi there"));

        if (Input.GetKeyDown(KeyCode.W))
            _taskSystemContainer.GetTaskSystem().AddTask(new PrintErrorTask("Oh hi there as an error"));

        if (Input.GetKeyDown(KeyCode.E))
            _taskSystemContainer.GetTaskSystem().AddTask(new PrintWithDelayTask("Oh hi there after 5 sec", 5f));
    }
}
