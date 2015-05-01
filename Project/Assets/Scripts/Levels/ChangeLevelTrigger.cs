using UnityEngine;
using System.Collections;

public class ChangeLevelTrigger : MonoBehaviour
{
    [SerializeField]
    private LevelsManager.ELevelName targetLevelName;
    [SerializeField]
    private PlayerSpawnPosition.EPlayerSpawnPosition targetSpawnPosition;

    private void OnTriggerEnter(Collider curCollider)
    {
        if (Zelda._Common._LevelsManager._CurLevelName != targetLevelName)
            Zelda._Common._LevelsManager._ChangeLevel(targetLevelName, new LevelInitLevelData(), new LocationInitLevelData(targetSpawnPosition));
        else
            Zelda._Common._LevelsManager._ChangeLocationOnLevel(new LocationInitLevelData(targetSpawnPosition));
    }
}