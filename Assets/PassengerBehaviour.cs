using System.Collections.Generic;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using UnityEngine;
using Utilities = Features.Building.Scripts.Datatypes.UtilityType;

public class PassengerBehaviour : MonoBehaviour
{
    [SerializeField] NPCController _npcController;
    Queue<Utilities> tasksToDo = new Queue<Utilities>();

    // Start is called before the first frame update
    void OnEnable()
    {
        AssignRandomTasks();
        ExecuteTasks();

        foreach (var item in tasksToDo)
        {
            print(item);
        }
    }

    void AssignRandomTasks()
    {
        // Uncomment this when there are more utilities than just security
        // tasksToDo.Clear();
        // int AmountOfUniqueTasks = System.Enum.GetValues(typeof(Utilities)).Length;

        // for (int i = 0; i < AmountOfUniqueTasks - 1; i++)
        // {
        //     Utilities taskType = (Utilities)i;
        //     int random = Random.Range(0, 2);
        //     if (random == 1) tasksToDo.Enqueue(taskType);
        // }

        //Security is required
        tasksToDo.Enqueue(Utilities.Security);
    }

    void ExecuteTasks()
    {
        if (tasksToDo.Count == 0) return;
        Utilities currentTask = tasksToDo.Dequeue();
        List<Vector3Int> potentialTaskDestinations = GameManager.Instance.GridManager.GetUtilities(currentTask);
        _npcController.SetTarget(potentialTaskDestinations[0], () => { }, () => { ExecuteTasks(); });
    }
}
