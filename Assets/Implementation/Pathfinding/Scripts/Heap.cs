using UnityEngine;
using Unity.Collections;
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

        /// <summary>
        /// Function that removed the item with the lowest value in the Heap and also removes it
        /// </summary>
        /// <returns>Node with the lowest F cost</returns>
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

        /// <summary>
        /// This handles moving A node down in the tree by comparing its f cost the its children
        /// </summary>
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

        /// <summary>
        /// Add a Node to the Heap, it gets added at the bottom of the tree and recursively moves itself up
        /// </summary>
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

        /// <summary>
        /// Move a node up in the tree by comparing its own fcost with its parent
        /// </summary>
        private void MoveUp(int index)
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

        /// <summary>
        /// Replace a node in the tree, then move it up and down so it is in the correct spot in the heap
        /// </summary>
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