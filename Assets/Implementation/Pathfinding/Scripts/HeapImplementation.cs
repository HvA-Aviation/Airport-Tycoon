using System.Collections;
using System.Collections.Generic;
using Implementation.Pathfinding.Scripts;
using UnityEngine;

public class HeapImplementation : MonoBehaviour
{
    public int amountOfNodes = 50;
    public Node currentNode;
    Heap heap;
    // Start is called before the first frame update
    void Start()
    {
        heap = new Heap(amountOfNodes);

        for (int i = 0; i < amountOfNodes; i++)
        {
            Node node = new Node() { fCost = Random.Range(0f, 50f) };
            heap.Add(node);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentNode = heap.Pop();
        }
    }

    private void OnApplicationQuit()
    {
        heap.DisposeOfLists();
    }
}
