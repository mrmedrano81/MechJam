using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] public PointSystem.EScoreSource scoreSource;
    [SerializeField] public UnityPointEvent deathEvent;

    private bool isDead;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject scoreManagerObject = GameObject.FindGameObjectWithTag("ScoreManager");

        if (scoreManagerObject != null)
        {
            ScoreManager scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
            deathEvent.AddListener(scoreManager.AddPoints);
        }
        else
        {
            Debug.Log("Null ScoreManager");
            Debug.Break();
        }

        isDead = false;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.Log("taking damage, health: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            deathEvent?.Invoke(scoreSource);
        }
    }

    public bool IsDead
    {
        get { return isDead; } 
    }
}
