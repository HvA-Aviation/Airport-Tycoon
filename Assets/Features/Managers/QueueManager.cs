using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    private Dictionary<Vector3Int, Queue<GameObject>> UtilityQueue = new Dictionary<Vector3Int, Queue<GameObject>>();

    private void Update()
    {
        // Debugging purposes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var item in UtilityQueue)
            {
                Debug.Log(item.Key + " " + item.Value.Count);
                RemoveFromQueue(item.Key);
            }
        }
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
            UtilityQueue[position].Enqueue(passenger);
        }

        return UtilityQueue[position].Count;
    }

    /// <summary>
    /// Remove a passenger from the queue
    /// </summary>
    public void RemoveFromQueue(Vector3Int position)
    {
        if (UtilityQueue[position].Count == 0) return;

        if (UtilityQueue.ContainsKey(position))
        {
            UpdatePositionOfQueuers(position);
            UtilityQueue[position].Dequeue();
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

    void UpdatePositionOfQueuers(Vector3Int dictKey)
    {
        Queue<GameObject> passengersInQueue = UtilityQueue[dictKey];
        Vector3 nextPosInQueue = dictKey;
        foreach (GameObject item in passengersInQueue)
        {
            // Update the position of the passengers in the queue
            Vector3 currentPosition = item.transform.position;
            StartCoroutine(WalkToPosition(item, nextPosInQueue));
            nextPosInQueue = currentPosition;
        }
    }
    IEnumerator WalkToPosition(GameObject gameObject, Vector3 targetPosition)
    {
        while (gameObject.transform.position != targetPosition)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, 0.01f);
            yield return null;
        }
    }
}
