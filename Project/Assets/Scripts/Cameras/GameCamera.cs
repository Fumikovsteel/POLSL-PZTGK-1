using UnityEngine;
using System.Collections;

public class GameCamera : ZeldaCamera
{
    private void Awake()
    {
        Zelda._Common._GameplayEvents._OnSceneWillChange += OnLevelWillChange;
    }

    private void OnLevelWillChange(SceneManager.ESceneName newScene)
    {
        // Because it has DontDestroyOnLoad
        if (newScene != SceneManager.ESceneName.Game)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Zelda._Common != null)
            Zelda._Common._GameplayEvents._OnSceneWillChange -= OnLevelWillChange;
    }
}
