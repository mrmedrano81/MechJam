using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] public PointSystem.EScoreSource scoreSource;
    [SerializeField] public UnityPointEvent deathEvent;
    [SerializeField] public UnityEvent integrityEvent;
    [SerializeField] public float randStickRange;

    private bool isDead;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject scoreManagerObject = GameObject.FindGameObjectWithTag("ScoreManager");

        if (scoreManagerObject != null)
        {
            ScoreManager scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
            deathEvent.AddListener(scoreManager.AddPoints);
            integrityEvent.AddListener(scoreManager.SubtractIntegrity);
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
        AudioManager.instance.PlayRandomSFX("RedBloodCell", 1f);
        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                Debug.Log("Dying");
                Debug.Break();
                isDead = true;
                deathEvent?.Invoke(scoreSource);
                integrityEvent?.Invoke();
                //Destroy(gameObject);
            }
        }
    }

    public bool IsDead
    {
        get { return isDead; } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, randStickRange);
    }
}
