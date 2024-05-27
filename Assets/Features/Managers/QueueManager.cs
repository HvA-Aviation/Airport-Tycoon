using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    private Dictionary<Vector3Int, Queue<GameObject>> UtilityQueue = new Dictionary<Vector3Int, Queue<GameObject>>();
    private Dictionary<Vector3Int, float> _queueProgression = new Dictionary<Vector3Int, float>();

    public int QueueCount(Vector3Int position)
    {
        return UtilityQueue[position].Count;
    }

    public bool QueueExists(Vector3Int position)
    {
        return UtilityQueue.ContainsKey(position);
    }

    /// <summary>
    /// Add a passenger to the queue
    /// </summary>
    public int AddToQueue(Vector3Int position, GameObject passenger)
    {
        if (UtilityQueue.ContainsKey(position))
        {
            UtilityQueue[position].Enqueue(passenger);
        }
        else
        {
            UtilityQueue.Add(position, new Queue<GameObject>());
            _queueProgression.Add(position, 0);
            UtilityQueue[position].Enqueue(passenger);
        }

        return UtilityQueue[position].Count;
    }

    public bool WorkOnQueue(Vector3Int position, float speed)
    {
        if (!UtilityQueue.ContainsKey(position) || UtilityQueue[position].Count == 0)
            return false;
        
        _queueProgression[position] += speed * Time.deltaTime;
        Debug.Log(_queueProgression[position]);
        return _queueProgression[position] >= 10f;
    }

    /// <summary>
    /// Remove a passenger from the queue
    /// </summary>
    public void RemoveFromQueue(Vector3Int position)
    {
        if (UtilityQueue.ContainsKey(position))
        {
            UtilityQueue[position].Dequeue();
            _queueProgression[position] = 0;
            UpdatePositionOfQueuers(position);
        }
    }

    /// <summary>
    /// Get the queue with the lowest amount of queuers
    /// </summary>
    /// <param name="potentialTaskDestinations">List of all the currently built destinations</param>
    public Vector3Int GetQueueWithLowestQueuers(List<Vector3Int> potentialTaskDestinations)
    {
        int lowestQueue = int.MaxValue;
        Vector3Int lowestQueuePosition = Vector3Int.zero;

        foreach (Vector3Int position in potentialTaskDestinations)
        {
            // Check if the position exists in the dictionary
            if (UtilityQueue.TryGetValue(position, out Queue<GameObject> queue))
            {
                if (queue.Count < lowestQueue)
                {
                    lowestQueue = queue.Count;
                    lowestQueuePosition = position;
                }
            }
            else
            {
                // If the position does not exist in the dictionary, add it
                UtilityQueue.Add(position, new Queue<GameObject>());
                _queueProgression.Add(position, 0);
                lowestQueuePosition = position;
            }
        }

        return lowestQueuePosition;
    }

    void UpdatePositionOfQueuers(Vector3Int dictPosition)
    {
        Queue<GameObject> passengersInQueue = UtilityQueue[dictPosition];
        Vector3 previousPosition = dictPosition;
        foreach (GameObject item in passengersInQueue)
        {
            // Update the position of the passengers in the queue
            Vector3 currentPosition = item.transform.position;
            item.transform.position = previousPosition;
            previousPosition = currentPosition;
        }
    }
}
