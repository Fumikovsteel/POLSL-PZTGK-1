using UnityEngine;
using System.Collections;
using System;

public class PlayerSpawnPosition : MonoBehaviour
{
    public enum EPlayerSpawnPosition
    {
        SpawnA, SpawnB, SpawnC, SpawnD, SpawnE
    }

    [SerializeField]
    private EPlayerSpawnPosition playerSpawnPositionId;
    [SerializeField]
    private LevelsManager.ELocationName locationName;

    private static string playerSpawnPositionsParentName = "PlayerSpawnPositions";

    public static PlayerSpawnPosition _GetSpawnPosition(EPlayerSpawnPosition playerSpawnPositionId, LevelsManager.ELocationName locationName)
    {
        GameObject enemySpawnPointsParent = GameObject.Find(playerSpawnPositionsParentName);
        if (enemySpawnPointsParent == null)
            Debug.LogError("You need to have " + playerSpawnPositionsParentName + " object on the scene!");
        PlayerSpawnPosition returnedObject = Array.Find<PlayerSpawnPosition>(enemySpawnPointsParent.GetComponentsInChildren<PlayerSpawnPosition>(),
                                                                             (x) => x.playerSpawnPositionId == playerSpawnPositionId &&
                                                                                    x.locationName == locationName);
        if (returnedObject == null)
            Debug.LogError("Given player spawn position id (" + playerSpawnPositionId + ") not found!");
        return returnedObject;
    }
}
