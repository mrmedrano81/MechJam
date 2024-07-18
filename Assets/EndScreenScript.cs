using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenScript : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject _playerHud;

    private void Start()
    {
        endPanel.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerHud.SetActive(false);
            endPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
