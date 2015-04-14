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
        GameInitLevelData gameInitLevelData = new GameInitLevelData();
        gameInitLevelData.gameType = GameInitLevelData.EStartGameType.newGame;

        Zelda._Common._SceneManager.ChangeScene(SceneManager.ESceneName.Game, gameInitLevelData);
    }

    public void OnContinueGameClicked()
    {
        GameInitLevelData gameInitLevelData = new GameInitLevelData();
        gameInitLevelData.gameType = GameInitLevelData.EStartGameType.continueGame;

        Zelda._Common._SceneManager.ChangeScene(SceneManager.ESceneName.Game, gameInitLevelData);
    }

    public void OnCreditsClicked()
    {
        EditorUtility.DisplayDialog("Credits", "Credits", "OK");
    }

    public void OnQuitGameClicked()
    {
        Zelda._Common._ApplicationManager.ExitApplication();
    }

    #endregion
}
