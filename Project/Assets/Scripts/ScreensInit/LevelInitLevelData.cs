using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class LevelInitLevelData : IInitLevelData
{
    [SerializeField]
    private PlayerSpawnPosition.EPlayerSpawnPosition targetSpawnPosition;

    public LevelInitLevelData(PlayerSpawnPosition.EPlayerSpawnPosition spawnPositionId)
    {
        targetSpawnPosition = spawnPositionId;
    }

    public PlayerSpawnPosition.EPlayerSpawnPosition _TargetSpawnPosition
    { get { return targetSpawnPosition; } }
}
