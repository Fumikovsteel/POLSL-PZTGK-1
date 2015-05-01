using UnityEngine;
using System.Collections;
using System;

public class PlayerSpawnPosition : MonoBehaviour
{
    public enum EPlayerSpawnPosition
    {
        firstLevelSlotA, firstLevelSlotB, firstLevelSlotC, secondLevelSlotA, thirdLevelSlotA
    }

    [SerializeField]
    private EPlayerSpawnPosition playerSpawnPositionId;

    private static string playerSpawnPositionsParentName = "PlayerSpawnPositions";

    public static PlayerSpawnPosition _GetSpawnPosition(EPlayerSpawnPosition playerSpawnPositionId)
    {
        GameObject enemySpawnPointsParent = GameObject.Find(playerSpawnPositionsParentName);
        if (enemySpawnPointsParent == null)
            Debug.LogError("You need to have " + playerSpawnPositionsParentName + " object on the scene!");
        PlayerSpawnPosition returnedObject = Array.Find<PlayerSpawnPosition>(enemySpawnPointsParent.GetComponentsInChildren<PlayerSpawnPosition>(),
                                                                             (x) => x.playerSpawnPositionId == playerSpawnPositionId);
        if (returnedObject == null)
            Debug.LogError("Given player spawn position id (" + playerSpawnPositionId + ") not found!");
        return returnedObject;
    }
}
