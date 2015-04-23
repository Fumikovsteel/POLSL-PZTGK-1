using UnityEngine;
using System.Collections;
using System;

public class GameInitLevelData : IInitLevelData
{
    public enum EStartGameType
    {
        newGame, continueGame
    }

    public EStartGameType gameType = EStartGameType.newGame;
}
