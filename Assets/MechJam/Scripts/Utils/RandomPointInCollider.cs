using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointInCollider : MonoBehaviour
{
    public static Vector2 GetRandomPointInCollider(Collider2D collider)
    {
        if (collider is BoxCollider2D)
        {
            return GetRandomPointInBoxCollider((BoxCollider2D)collider);
        }
        else if (collider is CircleCollider2D)
        {
            return GetRandomPointInCircleCollider((CircleCollider2D)collider);
        }
        else if (collider is PolygonCollider2D)
        {
            return GetRandomPointInPolygonCollider((PolygonCollider2D)collider);
        }
        else
        {
            throw new System.NotSupportedException("Collider type not supported");
        }
    }

    private static Vector2 GetRandomPointInBoxCollider(BoxCollider2D boxCollider)
    {
        Bounds bounds = boxCollider.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }

    private static Vector2 GetRandomPointInCircleCollider(CircleCollider2D circleCollider)
    {
        Vector2 point = Random.insideUnitCircle * circleCollider.radius;
        return (Vector2)circleCollider.transform.position + point;
    }

    private static Vector2 GetRandomPointInPolygonCollider(PolygonCollider2D polygonCollider)
    {
        Vector2 point;
        int attempts = 0;

        do
        {
            Bounds bounds = polygonCollider.bounds;
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            point = new Vector2(x, y);
            attempts++;
        } while (!polygonCollider.OverlapPoint(point) && attempts < 100);

        if (attempts >= 100)
        {
            Debug.LogWarning("Could not find a random point in the polygon collider after 100 attempts");
        }

        return point;
    }
}

