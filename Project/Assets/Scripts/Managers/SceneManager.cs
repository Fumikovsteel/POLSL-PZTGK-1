using UnityEngine;
using System.Collections;
using System;

public class SceneManager
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Structs

    public enum ESceneName
    {
        NotRegistered, MainMenu, Game
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    public ESceneName _CurScene;
    /// <summary>
    /// Returns true if we're on ours first scene
    /// </summary>
    public bool _FirstScene = true;
    /// <summary>
    /// returns singleton name for current scene
    /// </summary>
    public Zelda.ESingletonName _CurSceneSingleton
    {
        get
        {
            switch (_CurScene)
            {
                case ESceneName.Game:
                    return Zelda.ESingletonName.game;
                case ESceneName.MainMenu:
                    return Zelda.ESingletonName.mainMenu;
                default:
                    return Zelda.ESingletonName.empty;
            }
        }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private void UpdateCurScene()
    {
        if (Enum.IsDefined(typeof(ESceneName), Application.loadedLevelName))
            _CurScene = (ESceneName)Enum.Parse(typeof(ESceneName), Application.loadedLevelName);
        else
        {
            Debug.LogWarning("Current scene hasn't been registered!");
            _CurScene = ESceneName.NotRegistered;
        }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InitializationMethods

    public SceneManager()
    {
        UpdateCurScene();
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void ChangeScene(ESceneName targetScene, IInitLevelData initLevelData)
    {
        if (_CurScene == targetScene)
            return;

        Zelda._Common._InitLevelData = initLevelData;
        _FirstScene = false;
        Zelda._Common._GameplayEvents.RaiseOnLevelWillChange();
        Application.LoadLevel(targetScene.ToString());
    }

    /// <summary>
    /// event OnLevelWasLoaded, but raised before any others
    /// </summary>
    public void OnNewSceneLoaded()
    {
        if (_CurSceneSingleton != Zelda.ESingletonName.empty)
        {
            Zelda.ChangeState(_CurSceneSingleton, Zelda.ESingletonState.noAccess);
            UpdateCurScene();
            Zelda.ChangeState(_CurSceneSingleton, Zelda.ESingletonState.forceInitialize);
        }
    }

    #endregion
}
