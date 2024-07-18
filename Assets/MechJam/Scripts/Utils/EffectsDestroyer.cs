using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float delay;

    void Start()
    {
        Destroy(gameObject, delay);
    }
}
