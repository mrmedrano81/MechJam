using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_VirusAnimationScript : MonoBehaviour
{
    private Health health;
    private Movement movement;
    private Attack attack;

    private Animator anim;
    public GameObject particlePrefab;

    public string deathAnim;
    public float deathAnimdelay;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playAnim(string animName)
    {
        anim.Play(animName);
    }

    public void playDeathAnim()
    {
        anim.Play(deathAnim);
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Instantiate(particlePrefab, 
            transform.position + new Vector3(
                UnityEngine.Random.Range(-1, 1), 
                UnityEngine.Random.Range(-1, 1),
                UnityEngine.Random.Range(-1, 1)), 
                Quaternion.identity);
        Destroy(gameObject, deathAnimdelay);
    }
}

