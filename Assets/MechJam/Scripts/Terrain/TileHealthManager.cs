using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class DestructableTilemap
{
    public Dictionary<Vector3Int, float> healthTiles = new Dictionary<Vector3Int, float>();
    public Tilemap tilemap;
    public float maxHealth;
}

public class TileHealthManager : MonoBehaviour
{
    public UnityEvent onTileDestroyed;
    private Coroutine refreshGridRoutine;
    private bool refreshGrid;

    [SerializeField] private Tilemap cellBlockTilemap;
    [SerializeField] private float cellBlockHealth;
    [SerializeField] private Tilemap cholesterolBlockTilemap;
    [SerializeField] private float cholesterolBlockHealth;

    private DestructableTilemap cellBlockTiles;
    private DestructableTilemap cholesterolBlockTiles;

    private Dictionary<Tilemap, DestructableTilemap> breakableDict = new Dictionary<Tilemap, DestructableTilemap>();
    

    // Start is called before the first frame update
    void Start()
    {
        refreshGrid = false;
        GameObject AStarObject = GameObject.FindGameObjectWithTag("AStar");

        if (AStarObject != null)
        {
            Grid grid = AStarObject.GetComponent<Grid>();
            onTileDestroyed.AddListener(grid.CreateGrid);
            refreshGridRoutine = StartCoroutine(refreshGridCooldown());
        }
        else
        {
            Debug.Log("Null tilehealthmanager");
            Debug.Break();
        }

        cellBlockTiles = new DestructableTilemap();
        cellBlockTiles.tilemap = cellBlockTilemap;
        cellBlockTiles.maxHealth = cellBlockHealth;

        cholesterolBlockTiles = new DestructableTilemap();
        cholesterolBlockTiles.tilemap = cholesterolBlockTilemap;
        cholesterolBlockTiles.maxHealth = cholesterolBlockHealth;


        breakableDict.Add(cellBlockTilemap, cellBlockTiles);
        breakableDict.Add(cholesterolBlockTilemap, cholesterolBlockTiles);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeHealth(Vector2 worldPosition, float damage, Tilemap tilemap)
    {
        DestructableTilemap destructableTilemap = breakableDict[tilemap];
        Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);



        if (!destructableTilemap.healthTiles.ContainsKey(gridPosition))
        {
            destructableTilemap.healthTiles.Add(gridPosition, destructableTilemap.maxHealth);
        }

        float newValue = destructableTilemap.healthTiles[gridPosition] - damage;

        // tile death
        if (newValue <= 0f)
        {
            refreshGrid = true;
            tilemap.SetTile(gridPosition, null);
            destructableTilemap.healthTiles.Remove(gridPosition);
            if (refreshGridRoutine == null)
            {
                StartCoroutine(refreshGridCooldown());
            }
        }
        else
        {
            destructableTilemap.healthTiles[gridPosition] = newValue;
            Debug.Log("Health: "+ destructableTilemap.healthTiles[gridPosition]);
        }
    }

    IEnumerator refreshGridCooldown()
    {
        while(refreshGrid)
        {
            yield return new WaitForSeconds(0.5f);
            onTileDestroyed?.Invoke();
            refreshGrid = false;
        }
        yield break;
    }
}
