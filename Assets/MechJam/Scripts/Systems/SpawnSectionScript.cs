using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSectionScript : MonoBehaviour
{
    public GameObject[] mobList;

    private bool Spawned;

    private void Awake()
    {
        Spawned = false;
        foreach(GameObject mob in mobList)
        {
            mob.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Spawned == false)
        {
            foreach (GameObject mob in mobList)
            {
                if (mob != null) mob.SetActive(true);
            }
            Spawned=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Spawned == true)
        {
            foreach (GameObject mob in mobList)
            {
                if (mob != null) mob.SetActive(false);
            }
            Spawned = false;
        }
    }

}
