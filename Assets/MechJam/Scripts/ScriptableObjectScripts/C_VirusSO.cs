using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "C_VirusSO", menuName = "VirusSO/C_VirusSO")]
public class C_VirusSO : ScriptableObject
{
    public LayerMask[] targetLayers;
    public string[] targetTags;
}
