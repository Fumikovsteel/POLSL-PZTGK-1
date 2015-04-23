using UnityEngine;
using System.Collections;
using System;

public class LevelsManager
{
    public enum ELevelName
    {
        none, FirstLevel, SecondLevel, ThirdLevel
    }

    private ELevelName _GetLevelEnum(string levelName)
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

    public ELevelName _GetStartLevel(GameInitLevelData.EStartGameType startGameType, out LevelInitLevelData initLevelData)
    {
        switch (startGameType)
        {
            case GameInitLevelData.EStartGameType.newGame:
                initLevelData = new LevelInitLevelData(PlayerSpawnPosition.EPlayerSpawnPosition.firstLevelSlotA);
                return ELevelName.FirstLevel;
            case GameInitLevelData.EStartGameType.continueGame:
                Debug.LogWarning("Not implemented yet!"); goto default;
            default:
                initLevelData = null;
                return ELevelName.none;
        }
    }

    public void _ChangeLevel(ELevelName targetLevel, IInitLevelData initLevelData)
    {
        if (targetLevel == _GetLevelEnum(Application.loadedLevelName))
            return;

        Zelda._Common._InitLevelData = initLevelData;

        Zelda._Common._GameplayEvents.RaiseOnLevelWillChange(targetLevel);
        Zelda._Common._GameplayEvents.RaiseOnSceneWillChange(SceneManager.ESceneName.Game);
        Application.LoadLevel(targetLevel.ToString());
    }
}
