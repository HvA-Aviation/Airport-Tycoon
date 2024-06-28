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
    [SerializeField] Pax pax;
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

        int start = alreadyInQueue ? currentPathIndex : queuePositions.Count - 1;
        int end = positionInQueue;

        currentPathIndex = start;

        while (currentPathIndex >= end)
        {
            if (Vector3.Distance(queuePositions[currentPathIndex], transform.position) > distanceToNextPosComplete)
            {
                Vector3 direction = queuePositions[currentPathIndex] - transform.position;
                _npcController.Direction = direction.normalized;

                Vector3 queueSpeed = GameManager.Instance.QueueManager.queueProgressionSpeed *
                             GameManager.Instance.GameTimeManager.DeltaTime *
                             direction.normalized;

                transform.position += queueSpeed;
            }
            else
            {
                currentPathIndex--;
            }

            yield return new WaitForEndOfFrame();
        }

        //TODO make direction private and place movement in npc controller so 
        _npcController.Direction = Vector3.zero;
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
            GameManager.Instance.EventManager.SubscribeFlash(EventId.OnMissingUtility, (args) => { ExecuteTasks(); });
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
                GameManager.Instance.FinanceManager.Balance.Add(25);
                pax.Return();
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