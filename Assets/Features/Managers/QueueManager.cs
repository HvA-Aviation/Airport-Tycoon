using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public bool WorkOnQueue(Vector3Int utilityPos, float speed, float workLoad)
    {
        if (!_queueProgression.ContainsKey(utilityPos)) _queueProgression.Add(utilityPos, 0);

        _queueProgression[utilityPos] += speed * Time.deltaTime;
        return _queueProgression[utilityPos] >= workLoad;
    }

    /// <summary>
    /// Function to remove a passenger from the queue, also moves the queue along
    /// </summary>
    /// <param name="utilityPos"></param>
    public void RemoveFromQueue(Vector3Int utilityPos)
    {
        UpdatePositionOfQueuers(utilityPos);
        UtilityQueue[utilityPos].inQueue.Dequeue();
        _queueProgression[utilityPos] = 0;
    }

    /// <summary>
    /// Function that handles moving the passengers in a queue to move when someone leaves the queue
    /// </summary>
    void UpdatePositionOfQueuers(Vector3Int utilityPos)
    {
        List<PassengerBehaviour> passengersInQueue = UtilityQueue[utilityPos].inQueue.ToList();
        Vector3 nextPosInQueue = utilityPos;
        Vector3 currentPos;

        for (int i = 0; i < passengersInQueue.Count; i++)
        {
            currentPos = passengersInQueue[i].transform.position;
            StartCoroutine(WalkToPosition(passengersInQueue[i], nextPosInQueue));
            nextPosInQueue = currentPos;
        }
        UpdatePassengersNotInQueue(utilityPos);
    }

    /// <summary>
    /// Simple function that moves a gameobject from one position to another
    /// </summary>
    IEnumerator WalkToPosition(PassengerBehaviour gameObject, Vector3 targetPosition)
    {
        while (gameObject.transform.position != targetPosition)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, 0.01f);
            yield return null;
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

            if (UtilityQueue[item].inQueue.Count < currentBestCount)
            {
                currentBestCount = UtilityQueue[item].inQueue.Count;
                currentBestQueue = item;
            }
        }
        return currentBestQueue;
    }

    /// <summary>
    /// Assign a passenger to a utility and invoke its onQueueChanged action
    /// </summary>
    /// <param name="OnQueueChanged">This usually represents a pathfinding funciton</param>
    public void AssignToUtility(List<Vector3Int> utilityPos, PassengerBehaviour passenger, Action<int, Vector3Int> OnQueueChanged, out int positionInQueue)
    {
        Vector3Int optimalQueue = GetOptimalQueue(utilityPos);

        positionInQueue = UtilityQueue[optimalQueue].inQueue.Count;

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
        UpdatePassengersNotInQueue(utilityPos);
    }

    public void UpdatePassengersNotInQueue(Vector3Int utilityPos)
    {
        UtilityQueue.TryGetValue(utilityPos, out QueueInfo queueInfo);
        //Update the targets of everyone on their way to the queue
        foreach (var item in queueInfo.joiningQueue.ToList())
        {
            item.Value.Invoke(queueInfo.inQueue.Count, utilityPos);
        }
    }
}
