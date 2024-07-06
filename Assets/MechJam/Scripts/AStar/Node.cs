using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public int g_cost;
    public int h_cost;
    public int gridX;
    public int gridY;
    public bool walkable;

    public int heapIndex;

    public Vector3 worldPosition;

    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int f_cost
    {
        get { return g_cost + h_cost; }
    }

    public int HeapIndex 
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = f_cost.CompareTo(nodeToCompare.f_cost);
        if (compare == 0)
        {
            compare = h_cost.CompareTo(nodeToCompare.h_cost);
        }
        return -compare;
    }
}
