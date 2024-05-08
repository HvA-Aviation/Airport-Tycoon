using UnityEngine;

public class Demo : MonoBehaviour
{
    [SerializeField]
    private TaskSystemContainer _taskSystemContainer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _taskSystemContainer.GetTaskSystem().AddTask(new PrintTask("Oh hi there"));
        }
    }
}
