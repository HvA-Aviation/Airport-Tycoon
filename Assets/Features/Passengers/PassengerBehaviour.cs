using System.Collections.Generic;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using UnityEngine;
using Utilities = Features.Building.Scripts.Datatypes.UtilityType;
using Features.EventManager;
using System.Linq;
using UnityEngine.Tilemaps;

public class PassengerBehaviour : MonoBehaviour
{
    [SerializeField] NPCController _npcController;
    Queue<Utilities> tasksToDo = new Queue<Utilities>();
    QueueManager queueManager;
    GridManager gridManager;
    private Utilities _currentUtility;

    public bool atCorrectPositionInQueue = false;

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
        Dictionary<Vector3Int, List<Vector3Int>> potentialTaskDestinations = gridManager.GetUtilities(currentTask);

        foreach (var item in potentialTaskDestinations)
        {
            print(item.Key + " " + item.Value.Count);
        }

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

        print($"{rotation} at position {target}");

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

        target.z = 0;

        _npcController.SetTarget(
        target + rotationOffset,
        () => { },
        () => { GameManager.Instance.QueueManager.ReachedQueue(UtilityPos, this); },
        () => print("Implement function that handles case when passenger does not have path to target"));
    }
}