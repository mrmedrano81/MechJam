using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private bool isDead;

    // Start is called before the first frame update
    void Awake()
    {
        isDead = false;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("taking damage, health: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }

    public bool IsDead
    {
        get { return isDead; } 
    }
}
