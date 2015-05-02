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
                    Debug.LogWarning("Given scene not supported!");
                    return Zelda.ESingletonName.empty;
            }
        }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private ESceneName GetSceneEnum(string levelName)
    {
        if (Enum.IsDefined(typeof(ESceneName), levelName))
            return (ESceneName)Enum.Parse(typeof(ESceneName), levelName);
        else if (Enum.IsDefined(typeof(LevelsManager.ELevelName), levelName))
            return ESceneName.Game;
        else
        {
            Debug.LogWarning("Current scene hasn't been registered!");
            return ESceneName.NotRegistered;
        }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InitializationMethods

    public SceneManager()
    {
        _CurScene = GetSceneEnum(Application.loadedLevelName);
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void ChangeScene(ESceneName targetScene, IInitLevelData initLevelData)
    {
        if (_CurScene == targetScene)
            return;

        Zelda._Common.ClearInitLevelData();
        Zelda._Common.ChangeInitLevelData(initLevelData);

        _FirstScene = false;

        if (targetScene == ESceneName.Game)
        {
            LevelInitLevelData levelInitLevelData;
            LocationInitLevelData locationInitLevelData;
            LevelsManager.ELevelName startLevel = Zelda._Common._LevelsManager._GetStartLevel((initLevelData as GameInitLevelData).gameType, out levelInitLevelData, out locationInitLevelData);
            Zelda._Common._LevelsManager._ChangeLevel(startLevel, levelInitLevelData, locationInitLevelData);
        }
        else
        {
            Zelda._Common._GameplayEvents.RaiseOnSceneWillChange(GetSceneEnum(targetScene.ToString()));
            Application.LoadLevel(targetScene.ToString());
        }
    }

    /// <summary>
    /// event OnLevelWasLoaded, but raised before any others
    /// </summary>
    public void OnNewSceneLoaded()
    {
        if (_CurSceneSingleton != Zelda.ESingletonName.empty)
        {
            if (_CurScene != GetSceneEnum(Application.loadedLevelName))
            {
                Zelda.ChangeState(_CurSceneSingleton, Zelda.ESingletonState.noAccess);
                _CurScene = GetSceneEnum(Application.loadedLevelName);
                Zelda.ChangeState(_CurSceneSingleton, Zelda.ESingletonState.forceInitialize);
            }
        }
    }

    #endregion
}
