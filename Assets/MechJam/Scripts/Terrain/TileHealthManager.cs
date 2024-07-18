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
    public PointSystem.EScoreSource scoreSource;
}

public class TileHealthManager : MonoBehaviour
{
    private Coroutine refreshGridRoutine;
    private bool refreshGrid;

    [Header("Destructable Tilemap Setup")]
    [SerializeField] private Tilemap cellBlockTilemap;
    [SerializeField] private float cellBlockHealth;
    [SerializeField] private Tilemap cholesterolBlockTilemap;
    [SerializeField] private float cholesterolBlockHealth;

    [Header("Event Settings")]
    [SerializeField] private PointSystem.EScoreSource cellBlockScoreSource;
    [SerializeField] private PointSystem.EScoreSource cholesterolBlockScoreSource;

    private DestructableTilemap cellBlockTiles;
    private DestructableTilemap cholesterolBlockTiles;

    private Dictionary<Tilemap, DestructableTilemap> breakableDict = new Dictionary<Tilemap, DestructableTilemap>();

    [Header("Events")]
    public int refreshGridRadius;
    public UnityEvent onTileDestroyed;
    public UnityPointEvent deathEvent;
    public UnityEventRefreshGrid refreshGridEvent;

    [Header("Animation")]

    public TileAnimationScript anim;

    private void Awake()
    {
        anim = GetComponent<TileAnimationScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        refreshGrid = false;
        GameObject AStarObject = GameObject.FindGameObjectWithTag("AStar");

        if (AStarObject != null)
        {
            Grid grid = AStarObject.GetComponent<Grid>();
            //onTileDestroyed.AddListener(grid.CreateGrid);
            refreshGridEvent.AddListener(grid.RefreshGridSection);
        }
        else
        {
            Debug.Log("Null tilehealthmanager");
            Debug.Break();
        }

        GameObject scoreManagerObject = GameObject.FindGameObjectWithTag("ScoreManager");

        if (scoreManagerObject != null)
        {
            ScoreManager scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
            deathEvent.AddListener(scoreManager.AddPoints);
            onTileDestroyed.AddListener(scoreManager.SubtractIntegrity);
        }
        else
        {
            Debug.Log("Null ScoreManager");
            Debug.Break();
        }

        cellBlockTiles = new DestructableTilemap();
        cellBlockTiles.tilemap = cellBlockTilemap;
        cellBlockTiles.maxHealth = cellBlockHealth;
        cellBlockTiles.scoreSource = cellBlockScoreSource;

        cholesterolBlockTiles = new DestructableTilemap();
        cholesterolBlockTiles.tilemap = cholesterolBlockTilemap;
        cholesterolBlockTiles.maxHealth = cholesterolBlockHealth;
        cholesterolBlockTiles.scoreSource = cholesterolBlockScoreSource;


        breakableDict.Add(cellBlockTilemap, cellBlockTiles);
        breakableDict.Add(cholesterolBlockTilemap, cholesterolBlockTiles);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeHealth(Vector2 worldPosition, float damage, Tilemap tilemap)
    {
        //Debug.Log("ChangeHealthCalled");

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
            deathEvent?.Invoke(destructableTilemap.scoreSource);
            tilemap.SetTile(gridPosition, null);

            if (destructableTilemap.scoreSource == PointSystem.EScoreSource.CellBlock)
            {
                anim.playCellBlockDeath(worldPosition);
                onTileDestroyed?.Invoke();
            }
            else
            {
                anim.playCholBlockDeath(worldPosition);
            }
            destructableTilemap.healthTiles.Remove(gridPosition);

            refreshGridEvent?.Invoke(worldPosition, refreshGridRadius);
            refreshGridEvent?.Invoke(worldPosition, Mathf.RoundToInt(refreshGridRadius/2));
        }
        else
        {
            destructableTilemap.healthTiles[gridPosition] = newValue;
            //Debug.Log("Health: "+ destructableTilemap.healthTiles[gridPosition]);
        }
    }
}
