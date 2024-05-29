using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct QueueInfo
{
    public Queue<PassengerBehaviour> inQueue;
    public Dictionary<PassengerBehaviour, Action<int>> joiningQueue;
    public QueueInfo(PassengerBehaviour passenger, Action<int> onPositionInQueueChanged)
    {
        inQueue = new Queue<PassengerBehaviour>();
        joiningQueue = new Dictionary<PassengerBehaviour, Action<int>>
        {
            { passenger, onPositionInQueueChanged }
        };
    }
}

public class QueueManager : MonoBehaviour
{
    private Dictionary<Vector3Int, QueueInfo> UtilityQueue = new Dictionary<Vector3Int, QueueInfo>();
    private Dictionary<Vector3Int, float> _queueProgression = new Dictionary<Vector3Int, float>();

    public bool HasQueuers(Vector3Int utilityPos)
    {
        if (UtilityQueue.ContainsKey(utilityPos))
            return UtilityQueue[utilityPos].inQueue.Count > 0;
        else return false;
    }

    public bool WorkOnQueue(Vector3Int utilityPos, float speed)
    {
        if (!_queueProgression.ContainsKey(utilityPos)) _queueProgression.Add(utilityPos, 0);

        _queueProgression[utilityPos] += speed * Time.deltaTime;
        Debug.Log(_queueProgression[utilityPos]);
        return _queueProgression[utilityPos] >= 10f;
    }

    public void RemoveFromQueue(Vector3Int utilityPos)
    {
        UpdatePositionOfQueuers(utilityPos);
        UtilityQueue[utilityPos].inQueue.Dequeue();
        _queueProgression[utilityPos] = 0;
    }

    void UpdatePositionOfQueuers(Vector3Int utilityPos)
    {
        Queue<PassengerBehaviour> passengersInQueue = UtilityQueue[utilityPos].inQueue;
        Vector3 nextPosInQueue = utilityPos;
        foreach (PassengerBehaviour item in passengersInQueue)
        {
            // Update the position of the passengers in the queue
            Vector3 currentPosition = item.transform.position;
            StartCoroutine(WalkToPosition(item, nextPosInQueue));
            nextPosInQueue = currentPosition;
        }
    }

    IEnumerator WalkToPosition(PassengerBehaviour gameObject, Vector3 targetPosition)
    {
        while (gameObject.transform.position != targetPosition)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, 0.01f);
            yield return null;
        }
    }

    public void AssignToUtility(Vector3Int utilityPos, PassengerBehaviour passenger, Action<int> OnQueueChanged, out int positionInQueue)
    {
        if (!UtilityQueue.ContainsKey(utilityPos))
        {
            UtilityQueue.Add(utilityPos, new QueueInfo(passenger, OnQueueChanged));
            positionInQueue = UtilityQueue[utilityPos].inQueue.Count;
        }
        else
        {
            positionInQueue = UtilityQueue[utilityPos].inQueue.Count;
            UtilityQueue[utilityPos].joiningQueue.Add(passenger, OnQueueChanged);
        }
    }

    public void ReachedQueue(Vector3Int utilityPos, PassengerBehaviour passenger)
    {
        UtilityQueue[utilityPos].inQueue.Enqueue(passenger);
        UtilityQueue[utilityPos].joiningQueue.Remove(passenger);
        UpdateDestinations(utilityPos);
        print(passenger.transform.name + " reached the queue at " + utilityPos);
    }

    public void UpdateDestinations(Vector3Int utilityPos)
    {
        foreach (var item in UtilityQueue[utilityPos].joiningQueue.Values)
        {
            item?.Invoke(UtilityQueue[utilityPos].inQueue.Count);
        }
    }
}
