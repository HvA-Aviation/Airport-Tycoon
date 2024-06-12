using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

struct QueueInfo
{
    public Queue<PassengerBehaviour> InQueue;
    public Dictionary<PassengerBehaviour, Action<int, Vector3Int>> JoiningQueue;
    public QueueInfo(PassengerBehaviour passenger, Action<int, Vector3Int> onPositionInQueueChanged)
    {
        InQueue = new Queue<PassengerBehaviour>();
        JoiningQueue = new Dictionary<PassengerBehaviour, Action<int, Vector3Int>>
        {
            { passenger, onPositionInQueueChanged }
        };
    }
}

public class QueueManager : MonoBehaviour
{
    [Header("Settings"), Range(1, 10), Tooltip("How fast the queuers move to their next position")]
    public int SpeedOfQueuers;

    private Dictionary<Vector3Int, QueueInfo> _utilityQueue = new Dictionary<Vector3Int, QueueInfo>();
    private Dictionary<Vector3Int, float> _queueProgression = new Dictionary<Vector3Int, float>();


    /// <summary>
    /// Checks if there are any queuers at the specified utility position.
    /// </summary>
    /// <param name="utilityPos">The position of the utility to check.</param>
    /// <returns>True if there are queuers at the specified utility position, false otherwise.</returns>
    public bool HasQueuers(Vector3Int utilityPos)
    {
        if (_utilityQueue.ContainsKey(utilityPos))
            return _utilityQueue[utilityPos].InQueue.Count > 0;
        else
            return false;
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

        List<PassengerBehaviour> passengerBehaviours = queueInfo.InQueue.ToList();
        passengerBehaviours.AddRange(queueInfo.JoiningQueue.Keys);

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

        _queueProgression[utilityPos] += speed * Time.deltaTime;

        return _queueProgression[utilityPos] >= workLoad;
    }

    /// <summary>
    /// Removes a passenger from the queue at the specified utility position.
    /// </summary>
    /// <param name="utilityPos">The position of the utility in the queue.</param>
    public void RemoveFromQueue(Vector3Int utilityPos)
    {
        PassengerBehaviour passengerBehaviour = _utilityQueue[utilityPos].InQueue.Dequeue();
        StartCoroutine(UpdatePositionOfQueuers(utilityPos, passengerBehaviour.transform.position));
        passengerBehaviour.ExecuteTasks();

        _queueProgression[utilityPos] = 0;
        UpdateJoiningQueuers(utilityPos);
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
    /// <param name="utilityList">The list of utilities to choose from.</param>
    public Vector3Int GetOptimalQueue(List<Vector3Int> utilityList)
    {
        Vector3Int currentBestQueue = Vector3Int.zero;
        int currentBestCount = int.MaxValue;

        foreach (Vector3Int item in utilityList)
        {
            if (!_utilityQueue.ContainsKey(item))
            {
                QueueInfo queueInfo = new QueueInfo
                {
                    InQueue = new Queue<PassengerBehaviour>(),
                    JoiningQueue = new Dictionary<PassengerBehaviour, Action<int, Vector3Int>>()
                };
                _utilityQueue.TryAdd(item, queueInfo);
                return item;
            }

            int totalAmountOfQueuers = _utilityQueue[item].InQueue.Count + _utilityQueue[item].JoiningQueue.Count;
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
    /// <param name="utilityPos">The position of the utility.</param>
    /// <param name="passenger">The passenger to assign to the queue.</param>
    /// <param name="OnQueueChanged">The action to invoke when the queue changes.</param>
    public void AssignToUtility(List<Vector3Int> utilityPos, PassengerBehaviour passenger, Action<int, Vector3Int> OnQueueChanged)
    {
        Vector3Int optimalQueue = GetOptimalQueue(utilityPos);

        int positionInQueue = _utilityQueue[optimalQueue].InQueue.Count;

        _utilityQueue[optimalQueue].JoiningQueue.Add(passenger, OnQueueChanged);

        OnQueueChanged.Invoke(positionInQueue, optimalQueue);
    }

    /// <summary>
    /// Called when a passenger has reached the queue.
    /// </summary>
    /// <param name="utilityPos">The position of the utility.</param>
    /// <param name="passenger">The passenger that has reached the queue.</param>
    public void ReachedQueue(Vector3Int utilityPos, PassengerBehaviour passenger)
    {
        _utilityQueue.TryGetValue(utilityPos, out QueueInfo queueInfo);
        queueInfo.JoiningQueue.Remove(passenger);
        queueInfo.InQueue.Enqueue(passenger);
        UpdateJoiningQueuers(utilityPos);
    }

    /// <summary>
    /// Updates the joining queuers for a specific utility position.
    /// </summary>
    /// <param name="utilityPos">The position of the utility.</param>
    private void UpdateJoiningQueuers(Vector3Int utilityPos)
    {
        _utilityQueue.TryGetValue(utilityPos, out QueueInfo queueInfo);
        foreach (KeyValuePair<PassengerBehaviour, Action<int, Vector3Int>> item in queueInfo.JoiningQueue.ToList())
        {
            item.Value.Invoke(queueInfo.InQueue.Count(), utilityPos);
        }
    }
}