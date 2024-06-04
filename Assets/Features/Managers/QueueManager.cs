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
    public Queue<PassengerBehaviour> inQueue;
    public Dictionary<PassengerBehaviour, Action<int, Vector3Int>> joiningQueue;
    public QueueInfo(PassengerBehaviour passenger, Action<int, Vector3Int> onPositionInQueueChanged)
    {
        inQueue = new Queue<PassengerBehaviour>();
        joiningQueue = new Dictionary<PassengerBehaviour, Action<int, Vector3Int>>
        {
            { passenger, onPositionInQueueChanged }
        };
    }
}

public class QueueManager : MonoBehaviour
{
    private Dictionary<Vector3Int, QueueInfo> UtilityQueue = new Dictionary<Vector3Int, QueueInfo>();
    private Dictionary<Vector3Int, float> _queueProgression = new Dictionary<Vector3Int, float>();

    /// <summary>
    /// Function that returns true or false wether the queue exists and has passengers queueing
    /// </summary>
    public bool HasQueuers(Vector3Int utilityPos)
    {
        if (UtilityQueue.ContainsKey(utilityPos))
            return UtilityQueue[utilityPos].inQueue.Count > 0;
        else return false;
    }

    /// <summary>
    /// Function for guard to start letting people through a queue
    /// </summary>
    public bool WorkOnQueue(Vector3Int utilityPos, float speed)
    {
        if (!_queueProgression.ContainsKey(utilityPos)) _queueProgression.Add(utilityPos, 0);
        _queueProgression[utilityPos] += speed * Time.deltaTime;
        return _queueProgression[utilityPos] >= 10f;
    }

    /// <summary>
    /// Function to remove a passenger from the queue, also moves the queue along
    /// </summary>
    /// <param name="utilityPos"></param>
    public void RemoveFromQueue(Vector3Int utilityPos)
    {
        PassengerBehaviour passengerBehaviour = UtilityQueue[utilityPos].inQueue.Dequeue();
        StartCoroutine(UpdatePositionOfQueuers(utilityPos, passengerBehaviour.transform.position));
        passengerBehaviour.ExecuteTasks();

        _queueProgression[utilityPos] = 0;
        UpdateJoiningQueuers(utilityPos);
    }

    /// <summary>
    /// Function that handles moving the passengers in a queue to move when someone leaves the queue
    /// </summary>
    IEnumerator UpdatePositionOfQueuers(Vector3Int utilityPos, Vector3 originPos)
    {
        List<PassengerBehaviour> passengersInQueue = UtilityQueue[utilityPos].inQueue.ToList();
        Vector3 posToMoveTo = originPos;
        for (int i = 0; i < passengersInQueue.Count; i++)
        {
            Vector3 currentPos = passengersInQueue[i].transform.position;
            while (passengersInQueue[i].transform.position != posToMoveTo)
            {
                passengersInQueue[i].transform.position = Vector3.MoveTowards(passengersInQueue[i].transform.position, posToMoveTo, 10f * Time.deltaTime);
                yield return null;
            }
            posToMoveTo = currentPos;
        }
    }

    /// <summary>
    /// Function to get the utility that has the lowest amount of queuers
    /// </summary>
    /// <returns>A vector3Int that references to the position of the utility</returns>
    public Vector3Int GetOptimalQueue(List<Vector3Int> utilityList)
    {
        Vector3Int currentBestQueue = Vector3Int.zero;
        int currentBestCount = int.MaxValue;

        foreach (var item in utilityList)
        {
            if (!UtilityQueue.ContainsKey(item))
            {
                QueueInfo queueInfo = new QueueInfo
                {
                    inQueue = new Queue<PassengerBehaviour>(),
                    joiningQueue = new Dictionary<PassengerBehaviour, Action<int, Vector3Int>>()
                };
                UtilityQueue.TryAdd(item, queueInfo);
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
    /// Assign a passenger to a utility and invoke its onQueueChanged action
    /// </summary>
    /// <param name="OnQueueChanged">This usually represents a pathfinding funciton</param>
    public void AssignToUtility(List<Vector3Int> utilityPos, PassengerBehaviour passenger, Action<int, Vector3Int> OnQueueChanged)
    {
        Vector3Int optimalQueue = GetOptimalQueue(utilityPos);

        int positionInQueue = UtilityQueue[optimalQueue].inQueue.Count;

        UtilityQueue[optimalQueue].joiningQueue.Add(passenger, OnQueueChanged);

        OnQueueChanged.Invoke(positionInQueue, optimalQueue);
    }

    /// <summary>
    /// Function that notifies that the passenger has succesfully arrived at the queue.
    /// </summary>
    public void ReachedQueue(Vector3Int utilityPos, PassengerBehaviour passenger)
    {
        UtilityQueue.TryGetValue(utilityPos, out QueueInfo queueInfo);
        queueInfo.joiningQueue.Remove(passenger);
        queueInfo.inQueue.Enqueue(passenger);
        UpdateJoiningQueuers(utilityPos);
    }

    private void UpdateJoiningQueuers(Vector3Int utilityPos)
    {
        UtilityQueue.TryGetValue(utilityPos, out QueueInfo queueInfo);
        foreach (var item in queueInfo.joiningQueue.ToList())
        {
            item.Value.Invoke(queueInfo.inQueue.Count(), utilityPos);
        }
    }
}