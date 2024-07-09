using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;

    Grid grid;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos, endPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node current = openSet.RemoveFirst();
                closedSet.Add(current);

                if (current == targetNode)
                {
                    //sw.Stop();
                    //print("path found: " + sw.Elapsed.Milliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbours(current))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                    int newMoveCostToNeighbor = current.g_cost + GetDistance(current, neighbor) + neighbor.movementPenalty;

                    if (newMoveCostToNeighbor < neighbor.g_cost || !openSet.Contains(neighbor))
                    {
                        neighbor.g_cost = newMoveCostToNeighbor;
                        neighbor.h_cost = GetDistance(neighbor, targetNode);
                        neighbor.parent = current;

                        if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                        else openSet.UpdateItem(neighbor);
                    }
                }
            }
        }

        yield return null;

        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node current = endNode;

        while (current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }
        
        Vector3[] waypoints = SimplifyPath(path);
        
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

            if (directionOld != directionNew)
            {
                waypoints.Add(path[i].worldPosition);
            }
            else
            {
                //waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int x_dst = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int y_dst = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (x_dst > y_dst)
        {
            return 14 * x_dst + 10 * (y_dst - x_dst);
        }
        else
        {
            return 14 * y_dst + 10 * (x_dst - y_dst);
        }
    }
}
