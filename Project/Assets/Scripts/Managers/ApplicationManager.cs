using UnityEngine;
using System.Collections;

public class ApplicationManager
{
    public static bool _ApplicationIsQuitting = false;

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
