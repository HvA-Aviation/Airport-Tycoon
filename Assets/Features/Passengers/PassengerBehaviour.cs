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
    private Utilities _currentUtility;

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
        tasksToDo.Enqueue(Utilities.CheckIn);
        tasksToDo.Enqueue(Utilities.Security);
        tasksToDo.Enqueue(Utilities.Gate);
    }

    /// <summary>
    /// Execute the tasks that are assigned to a passenger
    /// </summary>
    public void ExecuteTasks(bool dequeue = true)
    {
        if (tasksToDo.Count == 0)
        {
            TasksCompleted();
            return;
        }

        Utilities currentTask = dequeue ? tasksToDo.Dequeue() : _currentUtility;
        List<Vector3Int> potentialTaskDestinations = gridManager.GetUtilities(currentTask);

        _currentUtility = currentTask;

        queueManager.AssignToUtility(
            potentialTaskDestinations,
            this,
            (numberInQueue, utilityPos) => { UpdatePath(utilityPos, numberInQueue); });
    }

    private void TasksCompleted()
    {
        switch (_currentUtility)
        {
            case Utilities.Gate:
                //TODO add finance manager here
                //GameManager.Instance.FinanceManager.Balance.Add(5);
                Destroy(gameObject);
                break;
        }
    }

    public void UpdatePath(Vector3Int utilityPos, int numberInQueue)
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