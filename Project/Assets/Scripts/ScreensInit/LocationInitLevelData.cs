using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class LocationInitLevelData : IInitLevelData
{
    [SerializeField]
    private PlayerSpawnPosition.EPlayerSpawnPosition targetSpawnPosition;

    public LocationInitLevelData(PlayerSpawnPosition.EPlayerSpawnPosition spawnPositionId)
    {
        targetSpawnPosition = spawnPositionId;
    }

    public PlayerSpawnPosition.EPlayerSpawnPosition _TargetSpawnPosition
    { get { return targetSpawnPosition; } }
}
