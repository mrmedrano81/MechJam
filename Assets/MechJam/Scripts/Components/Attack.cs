using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;
    public float dotDamage;
    public float s_dotDuration;
    public float s_dotInterval;
    public float range;

    private Coroutine DOTRoutine;

    public void DoDamage(Health targetHealth)
    {
        targetHealth.TakeDamage(damage);
    }

    public void DoDamageOverTime(Health targetHealth)
    {
        if (DOTRoutine == null)
        {
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
}
