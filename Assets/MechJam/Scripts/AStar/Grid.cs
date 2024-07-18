using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Grid : MonoBehaviour
{
    public bool displayGrid;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainType[] walkableRegions;
    public int blurSize;
    public int obstacleProximityPenalty;
    LayerMask walkableMask;
    Node[,] grid;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    [Header("Refresh Grid Settings")]
    public UnityEvent onTileDestroyed;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    int penaltyMin =  int.MaxValue;
    int penaltyMax = int.MinValue;

    private Camera cam;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach(TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2),region.terrainPenalty);
        }
        
    }


    private void Start()
    {
        CreateGrid();
    }
    private void Update()
    {
        //TrackCamera();
        //CreateGrid();
    }

    public int MaxSize
    {
        get 
        { 
            return gridSizeX*gridSizeY; 
        }
    }

    private void TrackCamera()
    {
        cam = Camera.main;
        transform.position = cam.transform.position;
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/ 2 - Vector3.up * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + 
                    Vector3.right * (x * nodeDiameter + nodeRadius) + 
                    Vector3.up * (y * nodeDiameter + nodeRadius);
                //bool walkable =  !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);

                int movementPenalty = 0;

                // raycast code
                RaycastHit2D[] hits = Physics2D.CircleCastAll(worldPoint, nodeRadius, (Vector2)transform.position, 0f, walkableMask);

                if (!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                if (walkable)
                {
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null)
                        {
                            walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                        }
                    }
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        BlurPenaltyMap(blurSize);
    }

    void BlurPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }
            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX-1);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x-1, y] 
                    - grid[removeIndex, y].movementPenalty
                    + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }
            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY - 1);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y-1]
                    - penaltiesHorizontalPass[x, removeIndex]
                    + penaltiesHorizontalPass[x, addIndex];

                int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }

    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public void RefreshGridSection(Vector3 collisionPoint, int radius)
    {
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        Node nearestNodeFromCollision = GetNodeFromWorldPoint(collisionPoint);

        float refreshOriginX = nearestNodeFromCollision.worldPosition.x;
        float refreshOriginY = nearestNodeFromCollision.worldPosition.y;

        int originGridPosX = nearestNodeFromCollision.gridX;
        int originGridPosY = nearestNodeFromCollision.gridY;

        
        int upper_searchRangeX = Mathf.Clamp((originGridPosX + radius), 0, gridSizeX);
        int lower_searchRangeX = Mathf.Clamp((originGridPosX - radius), 0, gridSizeX);        
        int upper_searchRangeY = Mathf.Clamp((originGridPosY + radius), 0, gridSizeY);
        int lower_searchRangeY = Mathf.Clamp((originGridPosY - radius), 0, gridSizeY);


        for (int x = lower_searchRangeX; x < upper_searchRangeX; x++)
        {
            for (int y = lower_searchRangeY; y < upper_searchRangeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft +
                    Vector3.right * (x * nodeDiameter + nodeRadius) +
                    Vector3.up * (y * nodeDiameter + nodeRadius);
                //bool walkable =  !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);

                int movementPenalty = 0;

                // raycast code
                RaycastHit2D[] hits = Physics2D.CircleCastAll(worldPoint, nodeRadius, (Vector2)transform.position, 0f, walkableMask);

                if (!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                if (walkable)
                {
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null)
                        {
                            walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                        }
                    }
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        //BlurPenaltyMap(blurSize);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y));

        if (grid != null && displayGrid)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, node.movementPenalty));

                Gizmos.color = (node.walkable) ? Gizmos.color : Color.red;
                Gizmos.DrawWireCube(node.worldPosition, Vector2.one * (nodeDiameter));
                //Gizmos.DrawCube(node.worldPosition, Vector2.one * (nodeDiameter));
            }

        }
    }

    [Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}


