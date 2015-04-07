using UnityEngine;
using System.Collections;
using UnityEditor;

public class MainMenuManager : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////
    #region InspectorProperties

    public GameObject EventSystem;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InitializationMethods

    private void Awake()
    {
        Instantiate(EventSystem);
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void OnStartGameClicked()
    {
        Zelda._Common._SceneManager.ChangeScene(SceneManager.ESceneName.Game, null);
    }

    public void OnContinueGameClicked()
    {
        EditorUtility.DisplayDialog("Continue", "Continue last game", "OK");
    }

    public void OnCreditsClicked()
    {
        EditorUtility.DisplayDialog("Credits", "Credits", "OK");
    }

    public void OnQuitGameClicked()
    {
        EditorUtility.DisplayDialog("Exit", "Game exit", "OK");
    }

    #endregion
}
