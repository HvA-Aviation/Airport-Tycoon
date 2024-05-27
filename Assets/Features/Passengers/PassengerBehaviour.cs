using System.Collections.Generic;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using UnityEngine;
using Utilities = Features.Building.Scripts.Datatypes.UtilityType;
using Grid = Features.Building.Scripts.Grid;

public class PassengerBehaviour : MonoBehaviour
{
    [SerializeField] NPCController _npcController;
    Queue<Utilities> tasksToDo = new Queue<Utilities>();
    bool isQueueing = false;

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

    /// <summary>
    /// Assign random tasks to the passenger
    /// </summary>
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

    /// <summary>
    /// Execute the tasks that are assigned to a passenger
    /// </summary>
    void ExecuteTasks()
    {
        if (tasksToDo.Count == 0) return;

        QueueManager queueManager = GameManager.Instance.QueueManager;
        GridManager gridManager = GameManager.Instance.GridManager;

        Utilities currentTask = tasksToDo.Dequeue();
        List<Vector3Int> potentialTaskDestinations = gridManager.GetUtilities(currentTask);

        Vector3Int queueWithLowestQueuers = queueManager.GetQueueWithLowestQueuers(potentialTaskDestinations);

        //Add to queue and get its index
        int numberInQueue = queueManager.AddToQueue(queueWithLowestQueuers, gameObject);

        Vector3Int positionInQueue = gridManager.GetRotation(queueWithLowestQueuers) switch
        {
            0 => Vector3Int.down * numberInQueue,
            1 => Vector3Int.left * numberInQueue,
            2 => Vector3Int.up * numberInQueue,
            3 => Vector3Int.right * numberInQueue,
            _ => Vector3Int.zero,
        };

        //Set target to the first destination in the list
        _npcController.SetTarget(queueWithLowestQueuers + positionInQueue, () => { }, () => { isQueueing = true; });
    }
}
