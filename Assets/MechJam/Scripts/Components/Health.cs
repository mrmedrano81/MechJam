using System.Collections;
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

    [Header("Hit animation")]
    private Coroutine flashRoutine;
    [SerializeField] public float flashDuration;
    [SerializeField] public Material flashingMaterial;
    [SerializeField] public Material originalMaterial;
    [SerializeField] public SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock _propertyBlock;
    private bool takingDamage;
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");



    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _propertyBlock = new MaterialPropertyBlock();

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
    private void Start()
    {
        originalMaterial = spriteRenderer.material;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.Log("taking damage, health: " + currentHealth);

        AudioManager.instance.PlayRandomSFX("RedBloodCell", 1f);

        Flash();

        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                //AudioManager.instance.PlayRandomSFX("CellBlock");
                isDead = true;
                deathEvent?.Invoke(scoreSource);
                integrityEvent?.Invoke();
                //Destroy(gameObject);
            }
        }
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(takingDamageAnimation());
    }

    private IEnumerator takingDamageAnimation()
    {
        //Debug.Log("Starting takingDamageAnimation coroutine");

        spriteRenderer.material = flashingMaterial;

        yield return new WaitForSeconds(flashDuration);

        if (isDead)
        {
            yield break;
        }

        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
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
