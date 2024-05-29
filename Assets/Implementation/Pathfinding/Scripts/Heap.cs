using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;
using System;
namespace Implementation.Pathfinding.Scripts
{
    public struct Heap
    {
        public NativeList<Node> items;
        public NativeHashMap<Vector3Int, Node> itemsMap;

        public Heap(int size)
        {
            items = new NativeList<Node>(size, Allocator.Persistent);
            itemsMap = new NativeHashMap<Vector3Int, Node>(size, Allocator.Persistent);
        }

        //Function HAS to be called or else we will get data leaks
        public void DisposeOfLists()
        {
            items.Dispose();
            itemsMap.Dispose();
        }

        public bool Contains(Vector3Int position) => itemsMap.ContainsKey(position);

        public Node GetAtPositionIndex(Vector3Int position) => itemsMap[position];

        public Node Pop()
        {
            itemsMap.Remove(items[0].position);

            // return new node if list is empty
            if (items.Length <= 0) return new Node();

            Node firstItem = items[0];
            items[0] = items[^1];
            items.RemoveAt(items.Length - 1);

            //Heapify large number down
            MoveDown(0);
            return firstItem;
        }

        private void MoveDown(int index)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;

            if (leftChildIndex >= items.Length || rightChildIndex >= items.Length) return;

            int smallerChildIndex = items[leftChildIndex].fCost > items[rightChildIndex].fCost ?
                                    rightChildIndex : leftChildIndex;
            Node smallerChild = items[smallerChildIndex];

            if (items[index].fCost > smallerChild.fCost)
            {
                Node temp = items[index];
                items[index] = smallerChild;
                items[smallerChildIndex] = temp;
                MoveDown(smallerChildIndex);
            }
        }

        public void Add(Node item)
        {
            itemsMap.TryAdd(item.position, item);
            // Special case for the first item
            if (items.Length <= 0)
            {
                items.Add(item);
                return;
            }

            // Special case for the second item 
            if (items.Length == 1)
            {
                items.Add(item);
                if (items[0].fCost > item.fCost)
                {
                    Node temp = items[0];
                    items[0] = item;
                    items[1] = temp;
                }
                else return;
            }

            // Add the item to the end of the list
            items.Add(item);
            int indexOfItem = items.Length - 1;
            MoveUp(indexOfItem);
        }

        public void MoveUp(int index)
        {
            // Get parents index
            int parentIndex = index / 2;
            if (items[parentIndex].fCost > items[index].fCost)
            {
                Node temp = items[parentIndex];
                items[parentIndex] = items[index];
                items[index] = temp;
                MoveUp(parentIndex);
            }
        }

        public void ReplaceNode(Vector3Int key, Node newNode)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (key == items[i].position)
                {
                    items[i] = newNode;

                    // Move it up and down
                    MoveUp(i);
                    MoveDown(i);
                }
            }
        }
    }
}