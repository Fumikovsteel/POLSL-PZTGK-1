using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    public int numberOfEnemies = 5;
    public Vector2 spawnWindow = new Vector2(5.0f, 5.0f);

    private void Start()
    {
        Transform enemiesParent = new GameObject("EnemiesParent").transform;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject instantiatedEnemy = (GameObject)Instantiate(enemyPrefab);
            instantiatedEnemy.transform.SetParent(enemiesParent, false);
            instantiatedEnemy.transform.localPosition = new Vector3(UnityEngine.Random.Range(-spawnWindow.x, spawnWindow.x),
                                                                    UnityEngine.Random.Range(-spawnWindow.y, spawnWindow.y), 0.0f);
        }
    }
}
