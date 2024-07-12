using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    public void Log(params object[] _messages)
    {
        string message = string.Join(" ", _messages);

        if (_showLogs)
        {
            Debug.Log(message);
        }
    }
}
