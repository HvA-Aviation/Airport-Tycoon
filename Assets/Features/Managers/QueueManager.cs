using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    private Dictionary<Vector3Int, Queue<GameObject>> UtilityQueue = new Dictionary<Vector3Int, Queue<GameObject>>();

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
            UtilityQueue[position].Enqueue(passenger);
        }

        return UtilityQueue[position].Count;
    }

    /// <summary>
    /// Remove a passenger from the queue
    /// </summary>
    public void RemoveFromQueue(Vector3Int position, GameObject passenger)
    {
        if (UtilityQueue.ContainsKey(position))
        {
            UtilityQueue[position].Dequeue();
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
