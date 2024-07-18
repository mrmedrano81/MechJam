using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointSystem
{
    public Dictionary<EScoreSource, float> scoreDict = new Dictionary<EScoreSource, float>();

    public enum EScoreSource
    {
        CellBlock,
        CholesterolBlock,
        RedBloodCell,
        WhiteBloodCell,
        A_Virus,
        C_Virus
    }

    public void AddScoreSource(EScoreSource sourceEnum, float points)
    {
        scoreDict.Add(sourceEnum, points);
    }

    public float GetSourcePoint(EScoreSource sourceEnum)
    {
        return scoreDict[sourceEnum];
    }
}

public class ScoreManager : MonoBehaviour
{
    [Header("Point Parameters")]
    public float cellBlock;
    public float cholesterolBlock;
    public float redBloodCell;
    public float whiteBloodCell;
    public float a_Virus;
    public float c_Virus;

    private PointSystem pointSystem;

    [Header("Score Variables")]
    public TMP_Text scoreDisplayText;
    private float totalScore;    
    public Image integrityBar;
    public float initialIntegrity;
    private float totalIntegrity;
    

    private void Awake()
    {
        pointSystem = new PointSystem();


    }

    private void Start()
    {
        pointSystem.AddScoreSource(PointSystem.EScoreSource.CellBlock, cellBlock);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.CholesterolBlock, cholesterolBlock);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.RedBloodCell, redBloodCell);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.WhiteBloodCell, whiteBloodCell);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.A_Virus, a_Virus);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.C_Virus, c_Virus);

        totalScore = 0;
        totalIntegrity = initialIntegrity/2;
    }

    private void Update()
    {
        if (scoreDisplayText != null)
        {
            if (totalScore < 0)
            {
                scoreDisplayText.color = Color.red;
                scoreDisplayText.text = totalScore.ToString();
            }
            else
            {
                scoreDisplayText.color = Color.green;
                scoreDisplayText.text = totalScore.ToString();
            }
        }
        if (integrityBar != null)
        {
            if (totalIntegrity <= 0)
            {
                SceneManager.LoadScene(0);
            }

            if (totalIntegrity < initialIntegrity/4)
            {
                integrityBar.color = Color.red;
                integrityBar.fillAmount = totalIntegrity / initialIntegrity;
            }
            else if (totalIntegrity < initialIntegrity / 2)
            {
                integrityBar.color = new Color(1f, 0.5f, 0f);
                integrityBar.fillAmount = totalIntegrity / initialIntegrity;
            }
            else
            {
                integrityBar.color = Color.green;
                integrityBar.fillAmount = totalIntegrity / initialIntegrity;
            }
        }
    }



    public void AddPoints(PointSystem.EScoreSource enumSource)
    {
        totalScore += pointSystem.GetSourcePoint(enumSource);
    }

    public void SubtractIntegrity()
    {
        totalIntegrity -= 20;
    }

    public void AddIntegrity()
    {
        totalIntegrity += 1;
    }    
    
    public void AddIntegrityFromVirusKill()
    {
        totalIntegrity += 30;
    }

}
