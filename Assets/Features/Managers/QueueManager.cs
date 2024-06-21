using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Managers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public struct QueueInfo
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
    private Dictionary<Vector3Int, QueueInfo> _utilityQueue = new Dictionary<Vector3Int, QueueInfo>();
    private Dictionary<Vector3Int, float> _queueProgression = new Dictionary<Vector3Int, float>();
    private Dictionary<Vector3Int, float> _queueTimeEstimate = new Dictionary<Vector3Int, float>();

    [Header("Settings")]
    [Tooltip("Speed at which the queuers move to their next position in queue")]
    [Range(1, 10)]
    public float queueProgressionSpeed = 1;

    void FixedUpdate()
    {
        CalculateQueueTimeEstimates();
    }

    public void CalculateQueueTimeEstimates()
    {
        foreach (var item in _utilityQueue)
        {
            if (!_queueTimeEstimate.ContainsKey(item.Key))
            {
                _queueTimeEstimate.Add(item.Key, 0f);
            }

            int amountOfPeopleInQueue = item.Value.inQueue.Count;
            int amountOfWalkingToQueue = item.Value.joiningQueue.Count;

            _queueTimeEstimate[item.Key] = (amountOfPeopleInQueue + amountOfWalkingToQueue) / 0.5f;
        }
    }

    public QueueInfo TryGetQueue(Vector3Int utilityPosition)
    {
        QueueInfo queueInfo = new QueueInfo();
        try
        {
            _utilityQueue.TryGetValue(utilityPosition, out queueInfo);
        }
        catch (KeyNotFoundException)
        {
            Debug.Log($"Key {utilityPosition} not found");
        }

        return queueInfo;
    }

    /// <summary>
    /// Checks if there are any queuers at the specified utility position.
    /// </summary>
    /// <param name="utilityPos">The position of the utility to check.</param>
    /// <returns>True if there are queuers at the specified utility position, false otherwise.</returns>
    public bool HasQueuers(Vector3Int utilityPos)
    {
        if (_utilityQueue.ContainsKey(utilityPos))
            return _utilityQueue[utilityPos].inQueue.Count > 0;
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

        QueueInfo queueInfo = _utilityQueue[utilityPos];

        List<PassengerBehaviour> passengerBehaviours = queueInfo.inQueue.ToList();
        passengerBehaviours.AddRange(queueInfo.joiningQueue.Keys);

        _utilityQueue.Remove(utilityPos);
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

        if (_utilityQueue[utilityPos].inQueue.First().atCorrectPositionInQueue)
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
        PassengerBehaviour passengerRemovedFromQueue = _utilityQueue[utilityPos].inQueue.Dequeue();

        int index = 0;
        foreach (PassengerBehaviour passenger in _utilityQueue[utilityPos].inQueue)
        {
            MoveToPositionInQueue(utilityPos, index, passenger, true);
            index++;
        }

        passengerRemovedFromQueue.ExecuteTasks();

        _queueProgression[utilityPos] = 0;
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

            int totalAmountOfQueuers = _utilityQueue[item].inQueue.Count + _utilityQueue[item].joiningQueue.Count;

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

        Vector3Int beginOfQueue = _utilityQueue[optimalQueue].queuePositions.Last();

        _utilityQueue[optimalQueue].joiningQueue.Add(passenger, OnQueueChanged);

        OnQueueChanged.Invoke(beginOfQueue, optimalQueue);
    }

    /// <summary>
    /// Called when a passenger has reached the queue.
    /// </summary>
    /// <param name="utilityPos">The position of the utility.</param>
    /// <param name="passenger">The passenger that has reached the queue.</param>
    public void ReachedQueue(Vector3Int utilityPos, PassengerBehaviour passenger)
    {
        _utilityQueue.TryGetValue(utilityPos, out QueueInfo queueInfo);
        queueInfo.joiningQueue.Remove(passenger);
        queueInfo.inQueue.Enqueue(passenger);
        MoveToPositionInQueue(utilityPos, _utilityQueue[utilityPos].inQueue.Count - 1, passenger);
    }

    /// <summary>
    /// Moves the passenger to the specified position in the queue.
    /// </summary>
    /// <param name="utilityPosition">The position of the utility in the queue.</param>
    /// <param name="positionInQueue">The position of the passenger in the queue.</param>
    /// <param name="passenger">The passenger to move.</param>
    public void MoveToPositionInQueue(Vector3Int utilityPosition, int positionInQueue, PassengerBehaviour passenger, bool alreadyInQueue = false)
    {
        List<Vector3Int> queuePositions = _utilityQueue[utilityPosition].queuePositions;
        passenger.MoveToTarget(queuePositions, positionInQueue, alreadyInQueue);
    }
}