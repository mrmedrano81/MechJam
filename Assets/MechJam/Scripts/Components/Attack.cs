using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;
    public float s_damageCooldown;
    public float dotDamage;
    public float s_dotDuration;
    public float s_dotInterval;
    public float range;

    private float s_timeSinceLastAttack;

    private Coroutine DOTRoutine;

    private void Awake()
    {
        s_timeSinceLastAttack = Time.time;
    }

    public void DoDamage(Health targetHealth)
    {
        if (Time.time - s_timeSinceLastAttack > s_damageCooldown)
        {
            Debug.Log((Time.time - s_timeSinceLastAttack) + " : " + s_damageCooldown);
            targetHealth.TakeDamage(damage);
            s_timeSinceLastAttack = Time.time;
        }
    }

    public void DoDamageOverTime(Health targetHealth)
    {
        if (DOTRoutine == null)
        {
            if (targetHealth == null)
            {
                Debug.Log("nullhealth");
            }

            DOTRoutine = StartCoroutine(ApplyDamageOverTime(targetHealth));
        }
    }

    private IEnumerator ApplyDamageOverTime(Health targetHealth)
    {
        float elapsedTime = 0f;
        while (elapsedTime < s_dotDuration)
        {
            if (targetHealth.IsDead || targetHealth == null) yield break;

            targetHealth.TakeDamage(damage);
            yield return new WaitForSeconds(s_dotInterval);
            elapsedTime += s_dotInterval;
        }
    }

    public Health GetHealthComponentIfInRange(LayerMask _layerMask, float _searchRadius, string tag = "None")
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _searchRadius, _layerMask);

        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (tag == "None")
                {
                    return hit.gameObject.GetComponent<Health>();
                }
                else if (hit.gameObject.CompareTag(tag))
                {
                    return hit.gameObject.GetComponent<Health>();
                }

                else return null;
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
