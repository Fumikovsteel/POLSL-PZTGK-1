using UnityEngine;
using System.Collections;
using System;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float defaultEnemiesHealth = 15.0f;
    [SerializeField]
    private float deathAnimationTime;
    [SerializeField]
    private Vector3 deathAnimationAmount = new Vector3(0.5f, 0.5f, 0.0f);

    private const string enemySpawnPointsParentName = "EnemySpawns";
    private Transform enemiesParent;
	ArrayList allSpawnPoints;

    private void Awake()
    {
        enemiesParent = new GameObject("EnemiesParent", typeof(DontDestroyOnLoad)).transform;

        Zelda._Common._GameplayEvents._OnSceneWillChange += OnSceneWillChange;
        Zelda._Common._GameplayEvents._OnLevelWasLoaded += OnLevelWasLoaded;
        Zelda._Common._GameplayEvents._OnPlayerWillBeKilled += OnPlayerWillBeKilled;
    }

    private void OnDestroy()
    {
        if (Zelda._Common != null)
        {
            Zelda._Common._GameplayEvents._OnSceneWillChange -= OnSceneWillChange;
            Zelda._Common._GameplayEvents._OnLevelWasLoaded -= OnLevelWasLoaded;
            Zelda._Common._GameplayEvents._OnPlayerWillBeKilled -= OnPlayerWillBeKilled;
        }
    }

    private void OnPlayerWillBeKilled()
    {
        enabled = false;
    }

	private void Update() 
	{
		SpawnEnemiesIfInRange ();
	}

    private void OnSceneWillChange(SceneManager.ESceneName newScene)
    {
        if (newScene != SceneManager.ESceneName.Game)
            Destroy(enemiesParent.gameObject);
    }

    private void OnLevelWasLoaded()
    {
		initSpawnPoints ();
    }

	private void SpawnEnemiesIfInRange()
	{
		if (allSpawnPoints == null) {
			initSpawnPoints();
		}
		Transform playerTransform = Zelda._Game._GameManager._Player.transform;
		for (int i = 0; i < allSpawnPoints.Count; i++)
		{
			EnemySpawnPoint spawnPoint = (EnemySpawnPoint) allSpawnPoints[i];
			var distance = Vector3.Distance(playerTransform.position, spawnPoint.transform.position);
			if(distance<spawnPoint._SpawnRange)
			{
				GameObject instantiatedEnemy = (GameObject)Instantiate(enemyPrefab);
				instantiatedEnemy.transform.SetParent(enemiesParent, false);
				instantiatedEnemy.transform.position = spawnPoint.transform.position;
                instantiatedEnemy.GetComponent<Enemy>()._Init(defaultEnemiesHealth);
				allSpawnPoints.RemoveAt(i);
			}
		}
	}

	private void initSpawnPoints() 
	{
		GameObject enemySpawnPointsParent = GameObject.Find(enemySpawnPointsParentName);
		if (enemySpawnPointsParent == null)
			Debug.LogError("You need to have " + enemySpawnPointsParentName + " object on the scene!");
		allSpawnPoints = new ArrayList(enemySpawnPointsParent.GetComponentsInChildren<EnemySpawnPoint>());
	}

    private void DestroyEnemy(Enemy enemyToDestroy)
    {
        Destroy(enemyToDestroy.gameObject);
    }

    public void _KillEnemy(Enemy enemyToKill)
    {
        enemyToKill._StopEnemyMovement();
        Action killAnimation = () =>
            {
                iTween.ShakePosition(enemyToKill.gameObject, iTween.Hash("amount", deathAnimationAmount, "time", deathAnimationTime, "islocal", true,
                                                                         "oncomplete", "DestroyEnemy", "oncompleteparams", enemyToKill,
                                                                         "oncompletetarget", gameObject));
            };
        // We delay because sword attack force need to change enemy position
        Zelda._Common._TimeManager._DelayMethodExecution(0.05f, killAnimation);
    }
}
