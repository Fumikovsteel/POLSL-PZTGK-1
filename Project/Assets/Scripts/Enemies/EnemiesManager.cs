using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    private const string enemySpawnPointsParentName = "EnemySpawns";
    private Transform enemiesParent;

    private void Awake()
    {
        enemiesParent = new GameObject("EnemiesParent", typeof(DontDestroyOnLoad)).transform;

        Zelda._Common._GameplayEvents._OnSceneWillChange += OnSceneWillChange;
        Zelda._Common._GameplayEvents._OnLevelWasLoaded += OnLevelWasLoaded;
    }

    private void OnDestroy()
    {
        if (Zelda._Common != null)
        {
            Zelda._Common._GameplayEvents._OnSceneWillChange -= OnSceneWillChange;
            Zelda._Common._GameplayEvents._OnLevelWasLoaded -= OnLevelWasLoaded;
        }
    }

    private void OnSceneWillChange(SceneManager.ESceneName newScene)
    {
        if (newScene != SceneManager.ESceneName.Game)
            Destroy(enemiesParent.gameObject);
    }

    private void OnLevelWasLoaded()
    {
        GameObject enemySpawnPointsParent = GameObject.Find(enemySpawnPointsParentName);
        if (enemySpawnPointsParent == null)
            Debug.LogError("You need to have " + enemySpawnPointsParentName + " object on the scene!");
        EnemySpawnPoint[] allSpawnPoints = enemySpawnPointsParent.GetComponentsInChildren<EnemySpawnPoint>();

        for (int i = 0; i < allSpawnPoints.Length; i++)
        {
            GameObject instantiatedEnemy = (GameObject)Instantiate(enemyPrefab);
            instantiatedEnemy.transform.SetParent(enemiesParent, false);
            instantiatedEnemy.transform.position = allSpawnPoints[i].transform.position;
        }
    }
}
