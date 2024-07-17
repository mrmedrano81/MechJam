using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject SettingsPanel;
    public void Paused()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void SettingsMenu()
    {
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void backButton()
    {
        PausePanel.SetActive(true);
        SettingsPanel.SetActive(false );
        Time.timeScale = 0;
    }
}
