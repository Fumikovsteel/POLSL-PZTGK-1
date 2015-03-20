using UnityEngine;
using System.Collections;

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

    #endregion
}
