using UnityEngine;

public static class DebugLogger
{
    public static void Log(string tag, string log, UnityEngine.Object context = null)
    {
        string logText = $"<color=blue>[{tag}]<color/> {log}";
        Debug.Log(logText, context);
    }

    public static void LogWarning(string tag, string log, UnityEngine.Object context = null)
    {
        string logText = $"<color=blue>[{tag}]<color/> {log}";
        Debug.LogWarning(logText, context);
    }

    public static void LogError(string tag, string log, UnityEngine.Object context = null)
    {
        string logText = $"<color=blue>[{tag}]<color/> {log}";
        Debug.LogError(logText, context);
    }
}