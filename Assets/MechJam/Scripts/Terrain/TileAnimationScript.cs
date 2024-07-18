using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimationScript : MonoBehaviour
{
    public GameObject cellBlockBreakPrefab;
    public GameObject cholBlockBreakPrefab;

    public void playCellBlockDeath(Vector3 targetPos)
    {
        Instantiate(cellBlockBreakPrefab, targetPos, Quaternion.identity);
    }
    public void playCholBlockDeath(Vector3 targetPos)
    {
        Instantiate(cholBlockBreakPrefab, targetPos, Quaternion.identity);
    }
}


