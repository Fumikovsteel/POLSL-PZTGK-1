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
        Zelda._Common._LevelsManager._ChangeLevel(targetLevelName, new LevelInitLevelData(targetSpawnPosition));
    }
}
