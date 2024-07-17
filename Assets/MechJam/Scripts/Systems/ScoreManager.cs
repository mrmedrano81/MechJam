using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float totalScore;

    private void Awake()
    {
        pointSystem = new PointSystem();

        pointSystem.AddScoreSource(PointSystem.EScoreSource.CellBlock, cellBlock);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.CholesterolBlock, cholesterolBlock);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.RedBloodCell, redBloodCell);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.WhiteBloodCell, whiteBloodCell);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.A_Virus, a_Virus);
        pointSystem.AddScoreSource(PointSystem.EScoreSource.C_Virus, c_Virus);
    }

    private void Start()
    {
        totalScore = 0;
    }

    public void AddPoints(PointSystem.EScoreSource enumSource)
    {
        totalScore += pointSystem.GetSourcePoint(enumSource);
    }

}
