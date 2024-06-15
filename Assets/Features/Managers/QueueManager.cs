using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

struct QueueInfo
{
    public Queue<PassengerBehaviour> inQueue;
    public Dictionary<PassengerBehaviour, Action<Vector3Int, Vector3Int>> joiningQueue;
    public List<Vector3Int> queuePositions;
    public QueueInfo(PassengerBehaviour passenger, Action<Vector3Int, Vector3Int> onPositionInQueueChanged)
    {
        inQueue = new Queue<PassengerBehaviour>();
        joiningQueue = new Dictionary<PassengerBehaviour, Action<Vector3Int, Vector3Int>>
        {
            { passenger, onPositionInQueueChanged }
        };
        queuePositions = new List<Vector3Int>();
    }
}

public class QueueManager : MonoBehaviour
{
    [Header("Settings"), Range(1, 10), Tooltip("How fast the queuers move to their next position")]
    public int SpeedOfQueuers;

    private Dictionary<Vector3Int, QueueInfo> _utilityQueue = new Dictionary<Vector3Int, QueueInfo>();
    private Dictionary<Vector3Int, float> _queueProgression = new Dictionary<Vector3Int, float>();

    [Header("Settings")]
    [Tooltip("Speed at which the queuers move to their next position in queue")]
    [Range(1, 10)]
    public float queueProgressionSpeed = 1;

    /// <summary>
    /// Checks if there are any queuers at the specified utility position.
    /// </summary>
    /// <param name="utilityPos">The position of the utility to check.</param>
    /// <returns>True if there are queuers at the specified utility position, false otherwise.</returns>
    public bool HasQueuers(Vector3Int utilityPos)
    {
        if (UtilityQueue.ContainsKey(utilityPos))
            return UtilityQueue[utilityPos].inQueue.Count > 0;
        else return false;
    }

    /// <summary>
    /// Removes a queue at the specified utility position.
    /// </summary>
    /// <param name="utilityPos">The position of the utility.</param>
    public void RemoveQueue(Vector3Int utilityPos)
    {
        if (!_utilityQueue.ContainsKey(utilityPos))
            return;

        QueueInfo queueInfo = UtilityQueue[utilityPos];

        List<PassengerBehaviour> passengerBehaviours = queueInfo.inQueue.ToList();
        passengerBehaviours.AddRange(queueInfo.joiningQueue.Keys);

        UtilityQueue.Remove(utilityPos);
        _queueProgression.Remove(utilityPos);

        foreach (PassengerBehaviour passenger in passengerBehaviours)
        {
            passenger.ExecuteTasks(false);
        }
    }


    /// <summary>
    /// Works on the queue at the specified utility position.
    /// </summary>
    /// <param name="utilityPos">The position of the utility.</param>
    /// <param name="speed">The speed at which the work is being done.</param>
    /// <param name="workLoad">The amount of work required to complete the queue.</param>
    /// <returns>True if the queue is completed, false otherwise.</returns>
    public bool WorkOnQueue(Vector3Int utilityPos, float speed, float workLoad)
    {
        if (!_queueProgression.ContainsKey(utilityPos))
            _queueProgression.Add(utilityPos, 0);

        if (UtilityQueue[utilityPos].inQueue.First().atCorrectPositionInQueue)
        {
            _queueProgression[utilityPos] += speed * Time.deltaTime;
        }

        return _queueProgression[utilityPos] >= workLoad;
    }

    /// <summary>
    /// Removes a passenger from the queue at the specified utility position.
    /// </summary>
    /// <param name="utilityPos">The position of the utility in the queue.</param>
    public void RemoveFromQueue(Vector3Int utilityPos)
    {
        PassengerBehaviour passengerBehaviour = UtilityQueue[utilityPos].inQueue.Dequeue();
        StartCoroutine(UpdatePositionOfQueuers(utilityPos, passengerBehaviour.transform.position));
        passengerBehaviour.ExecuteTasks();

        _queueProgression[utilityPos] = 0;
    }

    /// <summary>
    /// Updates the position of the queuers in the queue.
    /// </summary>
    /// <param name="utilityPos">The position of the utility in the queue.</param>
    /// <param name="originPos">The position of the utility.</param>
    IEnumerator UpdatePositionOfQueuers(Vector3Int utilityPos, Vector3 originPos)
    {
        List<PassengerBehaviour> passengersInQueue = _utilityQueue[utilityPos].InQueue.ToList();
        Vector3 posToMoveTo = originPos;
        for (int i = 0; i < passengersInQueue.Count; i++)
        {
            Vector3 currentPos = passengersInQueue[i].transform.position;
            while (passengersInQueue[i].transform.position != posToMoveTo)
            {
                PassengerBehaviour passenger = passengersInQueue[i];
                Vector3 currentPosition = passenger.transform.position;
                Vector3 targetPosition = posToMoveTo;
                float step = SpeedOfQueuers * Time.deltaTime;

                passenger.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, step);

                yield return null;
            }
            posToMoveTo = currentPos;
        }
    }

    /// <summary>
    /// Returns the optimal queue to join based on the amount of queuers in the queue.
    /// </summary>
    /// <returns>A vector3Int that references to the position of the utility</returns>
    public Vector3Int GetOptimalQueue(Dictionary<Vector3Int, List<Vector3Int>> utilityList)
    {
        Vector3Int currentBestQueue = Vector3Int.zero;
        int currentBestCount = int.MaxValue;

        foreach (var item in utilityList.Keys)
        {
            if (!_utilityQueue.ContainsKey(item))
            {
                QueueInfo queueInfo = new QueueInfo
                {
                    inQueue = new Queue<PassengerBehaviour>(),
                    joiningQueue = new Dictionary<PassengerBehaviour, Action<Vector3Int, Vector3Int>>(),
                    queuePositions = utilityList[item]
                };
                _utilityQueue.TryAdd(item, queueInfo);
                return item;
            }

            int totalAmountOfQueuers = UtilityQueue[item].inQueue.Count + UtilityQueue[item].joiningQueue.Count;
            if (totalAmountOfQueuers < currentBestCount)
            {
                currentBestCount = totalAmountOfQueuers;
                currentBestQueue = item;
            }
        }
        return currentBestQueue;
    }

    /// <summary>
    /// Assigns a passenger to the optimal queue.
    /// </summary>
    /// <param name="OnQueueChanged">This usually represents a pathfinding funciton</param>
    public void AssignToUtility(Dictionary<Vector3Int, List<Vector3Int>> utilityPos, PassengerBehaviour passenger, Action<Vector3Int, Vector3Int> OnQueueChanged)
    {
        Vector3Int optimalQueue = GetOptimalQueue(utilityPos);

        Vector3Int beginOfQueue = UtilityQueue[optimalQueue].queuePositions.LastOrDefault();

        UtilityQueue[optimalQueue].joiningQueue.Add(passenger, OnQueueChanged);

        OnQueueChanged.Invoke(beginOfQueue, optimalQueue);
    }

    /// <summary>
    /// Called when a passenger has reached the queue.
    /// </summary>
    /// <param name="utilityPos">The position of the utility.</param>
    /// <param name="passenger">The passenger that has reached the queue.</param>
    public void ReachedQueue(Vector3Int utilityPos, PassengerBehaviour passenger)
    {
        UtilityQueue.TryGetValue(utilityPos, out QueueInfo queueInfo);
        queueInfo.joiningQueue.Remove(passenger);
        queueInfo.inQueue.Enqueue(passenger);
        StartCoroutine(MoveToPositionInQueue(utilityPos, UtilityQueue[utilityPos].inQueue.Count - 1, passenger));
    }

    IEnumerator MoveToPositionInQueue(Vector3Int utilityPosition, int positionInQueue, PassengerBehaviour passenger)
    {
        passenger.atCorrectPositionInQueue = false;

        int totalQueueLength = UtilityQueue[utilityPosition].queuePositions.Count - 1;
        List<Vector3Int> Path = UtilityQueue[utilityPosition].queuePositions.GetRange(positionInQueue, totalQueueLength - positionInQueue);
        Path.Reverse();
        foreach (var item in Path)
        {
            print(item);
            while (Vector3.Distance(item, passenger.transform.position) > 0.1f)
            {
                Vector3 direction = item - passenger.transform.position;
                passenger.transform.position += queueProgressionSpeed * Time.deltaTime * direction.normalized;
                yield return new WaitForEndOfFrame();
            }
            passenger.transform.position = item;
        }

        passenger.atCorrectPositionInQueue = true;
    }
}