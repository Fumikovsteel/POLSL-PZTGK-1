using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class LocationInitLevelData : IInitLevelData
{
    [SerializeField]
    private PlayerSpawnPosition.EPlayerSpawnPosition targetSpawnPosition;
    [SerializeField]
    private LevelsManager.ELocationName targetLocationName;

    public LocationInitLevelData(PlayerSpawnPosition.EPlayerSpawnPosition spawnPositionId, LevelsManager.ELocationName locationName)
    {
        targetSpawnPosition = spawnPositionId;
        targetLocationName = locationName;
    }

    public PlayerSpawnPosition.EPlayerSpawnPosition _TargetSpawnPosition
    { get { return targetSpawnPosition; } }

    public LevelsManager.ELocationName _TargetLocationName
    { get { return targetLocationName; } }
}
