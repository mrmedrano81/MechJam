using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using static UnityEditor.Progress;
using Unity.VisualScripting;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node current = openSet.RemoveFirst();
            closedSet.Add(current);

            if (current == targetNode)
            {
                //sw.Stop();
                //print("path found: " + sw.Elapsed.Milliseconds + "ms");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbours(current))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                int newMoveCostToNeighbor = current.g_cost + GetDistance(current, neighbor);

                if (newMoveCostToNeighbor < neighbor.g_cost || !openSet.Contains(neighbor))
                {
                    neighbor.g_cost = newMoveCostToNeighbor;
                    neighbor.h_cost = GetDistance(neighbor, targetNode);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node current = endNode;

        while (current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int x_dst = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int y_dst = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (x_dst > y_dst)
        {
            return 14 * x_dst + 10 * (x_dst - y_dst);
        }
        else
        {
            return 14 * y_dst + 10 * (y_dst - x_dst);
        }
    }
}
