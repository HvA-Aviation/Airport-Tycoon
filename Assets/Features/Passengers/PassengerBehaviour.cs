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
    QueueManager queueManager;
    GridManager gridManager;

    // Start is called before the first frame update
    void OnEnable()
    {
        queueManager = GameManager.Instance.QueueManager;
        gridManager = GameManager.Instance.GridManager;
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

        Utilities currentTask = tasksToDo.Dequeue();
        List<Vector3Int> potentialTaskDestinations = gridManager.GetUtilities(currentTask);

        queueManager.AssignToUtility(
            potentialTaskDestinations,
            this,
            (numberInQueue, utilityPos) => { UpdatePath(utilityPos, numberInQueue); },
            out int positionInQueue);
    }

    void UpdatePath(Vector3Int utilityPos, int numberInQueue)
    {
        _npcController.StopAllCoroutines();

        int rotation = gridManager.GetRotation(utilityPos);
        Vector3Int rotationVector = Vector3Int.down;

        switch (rotation)
        {
            case 0:
                rotationVector = Vector3Int.down;
                break;
            case 1:
                rotationVector = Vector3Int.left;
                break;
            case 2:
                rotationVector = Vector3Int.up;
                break;
            case 3:
                rotationVector = Vector3Int.right;
                break;
        }

        _npcController.SetTarget(
        utilityPos + rotationVector * (numberInQueue + 1),
        () => { },
        () => { GameManager.Instance.QueueManager.ReachedQueue(utilityPos, this); });
    }

}