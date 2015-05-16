using UnityEngine;
using System.Collections;
using UnityEditor;

public class MainMenuManager : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////
    #region InspectorProperties

    public GameObject EventSystem;
    private GameObject credits = null;

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
        Zelda._Common._SceneManager.ChangeScene(SceneManager.ESceneName.Game, new GameInitLevelData() { gameType = GameInitLevelData.EStartGameType.newGame });
    }

    public void OnContinueGameClicked()
    {
        Zelda._Common._SceneManager.ChangeScene(SceneManager.ESceneName.Game, new GameInitLevelData() { gameType = GameInitLevelData.EStartGameType.continueGame });
    }
    
    public void OnCreditsClicked()
    {
        if (credits)
        {
            credits.SetActive(true);
        }
        else
        {
            credits = Zelda._Common._ResourcesManager.InstantiatePrefab(ResourcesManager.EPrefabName.CreditsManager, null);
        }
    }

    public void OnQuitGameClicked()
    {
        Zelda._Common._ApplicationManager.ExitApplication();
    }

    public void OnCloseCreditsBtnClicked()
    {
        //Destroy(creditsWindow);
    }


    #endregion
}
