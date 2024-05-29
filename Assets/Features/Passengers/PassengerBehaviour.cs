using System.Collections.Generic;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using UnityEngine;
using Utilities = Features.Building.Scripts.Datatypes.UtilityType;
using Grid = Features.Building.Scripts.Grid;
using System;

public class PassengerBehaviour : MonoBehaviour
{
    [SerializeField] NPCController _npcController;
    Queue<Utilities> tasksToDo = new Queue<Utilities>();
    public bool IsQueueing { get; private set; }

    // Start is called before the first frame update
    void OnEnable()
    {
        AssignRandomTasks();
        ExecuteTasks();
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

        queueManager.AssignToUtility(
            potentialTaskDestinations[0],
            this,
            (numberInQueue) => { UpdatePath(numberInQueue, potentialTaskDestinations[0]); },
            out int positionInQueue);

        _npcController.SetTarget(
            potentialTaskDestinations[0] + Vector3Int.down * (positionInQueue + 1),
            () => { },
            () => { queueManager.ReachedQueue(potentialTaskDestinations[0], this); });
    }

    public void UpdatePath(int numberInQueue, Vector3Int destination)
    {
        _npcController.StopAllCoroutines();
        _npcController.SetTarget(
            destination + Vector3Int.down * (numberInQueue + 1),
            () => { },
            () => { GameManager.Instance.QueueManager.ReachedQueue(destination, this); });
    }
}