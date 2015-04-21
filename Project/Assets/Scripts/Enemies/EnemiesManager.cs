using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    private const string enemySpawnPointsParentName = "EnemySpawns";
    private EnemySpawnPoint[] allSpawnPoints;

    private void Awake()
    {
        GameObject enemySpawnPointsParent = GameObject.Find(enemySpawnPointsParentName);
        if (enemySpawnPointsParent == null)
            Debug.LogError("You need to have " + enemySpawnPointsParentName + " object on the scene!");
        allSpawnPoints = enemySpawnPointsParent.GetComponentsInChildren<EnemySpawnPoint>();
    }

    private void Start()
    {
        Transform enemiesParent = new GameObject("EnemiesParent").transform;
        for (int i = 0; i < allSpawnPoints.Length; i++)
        {
            GameObject instantiatedEnemy = (GameObject)Instantiate(enemyPrefab);
            instantiatedEnemy.transform.SetParent(enemiesParent, false);
            instantiatedEnemy.transform.position = allSpawnPoints[i].transform.position;
        }
    }
}
