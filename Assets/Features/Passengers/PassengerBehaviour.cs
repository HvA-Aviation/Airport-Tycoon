using System.Collections.Generic;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using UnityEngine;
using Utilities = Features.Building.Scripts.Datatypes.UtilityType;
using Features.EventManager;
using System.Collections;

public class PassengerBehaviour : MonoBehaviour
{
    [SerializeField] NPCController _npcController;
    Queue<Utilities> tasksToDo = new Queue<Utilities>();
    QueueManager queueManager;
    GridManager gridManager;
    private Utilities _currentUtility;

    [Header("Queue variables")]
    public bool atCorrectPositionInQueue = false;
    public float distanceToNextPosComplete = 0.1f;
    public int currentPathIndex;

    void OnEnable()
    {
        queueManager = GameManager.Instance.QueueManager;
        gridManager = GameManager.Instance.GridManager;
        transform.position = gridManager.GetPaxSpawnPoint();
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

    public void MoveToTarget(List<Vector3Int> queuePositions, int positionInQueue, bool alreadyInQueue = false)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPositionInQueue(queuePositions, positionInQueue, alreadyInQueue));
    }

    public IEnumerator MoveToPositionInQueue(List<Vector3Int> queuePositions, int positionInQueue, bool alreadyInQueue = false)
    {
        atCorrectPositionInQueue = false;

        int totalQueueLength = queuePositions.Count - 1;

        int start = alreadyInQueue ? currentPathIndex : totalQueueLength;
        int end = positionInQueue;

        int pathIndex = start;

        currentPathIndex = pathIndex;

        while (pathIndex != end && !atCorrectPositionInQueue)
        {

            if (pathIndex < 0 || pathIndex > totalQueueLength)
            {
                print("Path index out of bounds" + pathIndex + " " + totalQueueLength);
                yield break;
            }

            if (Vector3.Distance(queuePositions[pathIndex], transform.position) < distanceToNextPosComplete)
            {
                pathIndex--;
                currentPathIndex = pathIndex;
            }

            Vector3 direction = queuePositions[pathIndex] - transform.position;
            Vector3 queueSpeed = GameManager.Instance.QueueManager.queueProgressionSpeed *
                                 GameManager.Instance.GameTimeManager.DeltaTime *
                                 direction.normalized;

            transform.position += queueSpeed;

            yield return null;
        }

        atCorrectPositionInQueue = true;
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
        Dictionary<Vector3Int, List<Vector3Int>> potentialTaskDestinations = gridManager.GetUtilities(currentTask);

        // Check if there are any utilities of the current task, if not then subscribe to the onMissingUtility event
        if (potentialTaskDestinations.Count == 0)
        {
            Debug.LogWarning($"Missing a {currentTask} to assign to subscribing to onMissingUtility event");
            GameManager.Instance.EventManager.SubscribeFlash(EventId.onMissingUtility, (args) => { ExecuteTasks(); });
            return;
        }

        _currentUtility = currentTask;

        queueManager.AssignToUtility(
            potentialTaskDestinations,
            this,
            (target, utilityPos) => { UpdatePath(target, utilityPos); });
    }

    /// <summary>
    /// Handles the completion of tasks for the passenger.
    /// </summary>
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

    public void UpdatePath(Vector3Int target, Vector3Int UtilityPos)
    {
        _npcController.StopAllCoroutines();

        // Rotation doesn't function properly on queues
        int rotation = gridManager.GetRotation(target);

        Vector3Int rotationOffset = Vector3Int.zero;

        switch (rotation)
        {
            case 0:
                rotationOffset = Vector3Int.down;
                break;
            case 1:
                rotationOffset = Vector3Int.left;
                break;
            case 2:
                rotationOffset = Vector3Int.up;
                break;
            case 3:
                rotationOffset = Vector3Int.right;
                break;
        }

        _npcController.SetTarget(
        target + rotationOffset,
        () => { },
        () => { GameManager.Instance.QueueManager.ReachedQueue(UtilityPos, this); },
        () => print("Implement function that handles case when passenger does not have path to target"));
    }
}