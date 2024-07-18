using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructableTiles : MonoBehaviour
{
    public Tilemap destructibleTileMap;

    public TileHealthManager healthManager;
    public string[] interactionTags;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        destructibleTileMap = GetComponent<Tilemap>();
    }

    public void DamageTile(Collision2D collision, float damage)
    {
        Vector3 hitPos = Vector3.zero;
        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPos.x = hit.point.x - 0.01f * hit.normal.x;
            hitPos.y = hit.point.y - 0.01f * hit.normal.y;
            healthManager.ChangeHealth(hitPos, damage, destructibleTileMap);
        }

        //ContactPoint2D hit = collision.contacts[0];
        //hitPos.x = hit.point.x - 0.01f * hit.normal.x;
        //hitPos.y = hit.point.y - 0.01f * hit.normal.y;
        //Debug.Log("hit: (" + hitPos.x + ", " + hitPos.y + ")");
        //healthManager.ChangeHealth(hitPos, damage, destructibleTileMap);
    }

    public void DamageTileFromRaycast(Vector2 hitPoint, Vector2 hitNormal, float damage)
    {
        Vector3 hitPos = Vector3.zero;

        // Adjust the hit position based on the normal
        hitPos.x = hitPoint.x - 0.01f * hitNormal.x;
        hitPos.y = hitPoint.y - 0.01f * hitNormal.y;

        //Debug.Log($"Hit position: ({hitPos.x}, {hitPos.y})");

        // Pass the adjusted hit position to your health manager
        healthManager.ChangeHealth(hitPos, damage, destructibleTileMap);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);

        if (interactionTags.Length > 0)
        {
            foreach (string tag in interactionTags)
            {
                if (collision.gameObject.CompareTag(tag))
                {
                    float damage = collision.gameObject.GetComponent<Attack>().damage;

                    DamageTile(collision, damage);
                }
            }
        }
    }
}
