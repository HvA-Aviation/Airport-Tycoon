using System.Collections.Generic;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using UnityEngine;
using Utilities = Features.Building.Scripts.Datatypes.UtilityType;
using System;
using System.Linq;

public class PassengerBehaviour : MonoBehaviour
{
    [SerializeField] NPCController _npcController;
    Queue<Utilities> tasksToDo = new Queue<Utilities>();
    QueueManager queueManager;
    GridManager gridManager;
    private Utilities _currentUtility;

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
        //TODO: Make a system that randomly assigns tasks when more utilities are added

        //Security is required
        tasksToDo.Enqueue(Utilities.CheckIn);
        tasksToDo.Enqueue(Utilities.Security);
        tasksToDo.Enqueue(Utilities.Gate);
    }

    /// <summary>
    /// Executes the tasks assigned to the passenger.
    /// </summary>
    /// <param name="dequeue">Indicates whether to dequeue the next task from the task queue.</param>
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

    /// <summary>
    /// Handles the completion of tasks for the passenger.
    /// </summary>
    private void TasksCompleted()
    {
        switch (_currentUtility)
        {
            case Utilities.Gate:
                GameManager.Instance.FinanceManager.Balance.Add(5);
                Destroy(gameObject);
                break;
        }
    }

    /// <summary>
    /// Updates the path for the passenger's movement.
    /// </summary>
    /// <param name="utilityPos">The position of the utility the passenger is heading towards.</param>
    /// <param name="numberInQueue">The number of passengers in the queue before this passenger.</param>
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

        // Calculate the target position
        Vector3Int targetPosition = utilityPos + rotationVector * (numberInQueue + 1);

        // Define the action to be performed when the target is reached
        Action onTargetReached = () => { GameManager.Instance.QueueManager.ReachedQueue(utilityPos, this); };

        // Define the action to be performed if the target cannot be reached
        Action onTargetUnreachable = () => { };

        // Set the target for the NPC controller
        _npcController.SetTarget(targetPosition, onTargetUnreachable, onTargetReached);
    }

}