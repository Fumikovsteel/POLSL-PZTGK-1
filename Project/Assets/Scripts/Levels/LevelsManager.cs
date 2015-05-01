using UnityEngine;
using System.Collections;
using System;

public class LevelsManager
{
    public enum ELevelName
    {
        none, FirstLevel, SecondLevel, ThirdLevel
    }

    public LevelsManager()
    {
        Zelda._Common._GameplayEvents._OnLevelWasLoaded += OnLevelWasLoaded;
    }

    ~LevelsManager()
    {
        if (Zelda._Common != null)
            Zelda._Common._GameplayEvents._OnLevelWasLoaded -= OnLevelWasLoaded;
    }

    private void OnLevelWasLoaded()
    {
        _CurLevelName = GetLevelEnum(Application.loadedLevelName);
        if (Zelda._Common._SceneManager._CurScene == SceneManager.ESceneName.Game)
            Zelda._Common._GameplayEvents.RaiseOnLocationChanged();
    }

    private ELevelName GetLevelEnum(string levelName)
    {
        if (Enum.IsDefined(typeof(ELevelName), levelName))
            return (ELevelName)Enum.Parse(typeof(ELevelName), levelName);
        // If we change scene from main menu to first scene for example
        else if (Enum.IsDefined(typeof(SceneManager.ESceneName), levelName))
            return ELevelName.none;
        else
        {
            Debug.LogWarning("Current level hasn't been registered!");
            return ELevelName.none;
        }
    }

    public ELevelName _CurLevelName
    {
        get;
        private set;
    }

    public ELevelName _GetStartLevel(GameInitLevelData.EStartGameType startGameType, out LevelInitLevelData initLevelData, out LocationInitLevelData initLocationData)
    {
        switch (startGameType)
        {
            case GameInitLevelData.EStartGameType.newGame:
                initLevelData = new LevelInitLevelData();
                initLocationData = new LocationInitLevelData(PlayerSpawnPosition.EPlayerSpawnPosition.firstLevelSlotA);
                return ELevelName.FirstLevel;
            case GameInitLevelData.EStartGameType.continueGame:
                Debug.LogWarning("Not implemented yet!"); goto default;
            default:
                initLevelData = null;
                initLocationData = null;
                return ELevelName.none;
        }
    }

    public void _ChangeLevel(ELevelName targetLevel, LevelInitLevelData initLevelData, LocationInitLevelData locationInitLevelData)
    {
        if (targetLevel == _CurLevelName)
            return;

        Zelda._Common.ChangeInitLevelData(initLevelData);
        Zelda._Common.ChangeInitLevelData(locationInitLevelData);

        Zelda._Common._GameplayEvents.RaiseOnLevelWillChange(targetLevel);
        Zelda._Common._GameplayEvents.RaiseOnSceneWillChange(SceneManager.ESceneName.Game);
        Zelda._Common._GameplayEvents.RaiseOnLocationWillChange();
        Application.LoadLevel(targetLevel.ToString());
    }

    public void _ChangeLocationOnLevel(LocationInitLevelData locationInitData)
    {
        Zelda._Common._GameplayEvents.RaiseOnLocationWillChange();
        Zelda._Common.ChangeInitLevelData(locationInitData);
        Zelda._Common._GameplayEvents.RaiseOnLocationChanged();
    }
}
