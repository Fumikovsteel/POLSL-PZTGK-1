using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject eventSystem;

    private GameObject instantiatedEventSystem;

    private void Awake()
    {
        instantiatedEventSystem = Instantiate(eventSystem) as GameObject;
    }

    private void OnDestroy()
    {
        if (instantiatedEventSystem != null)
            Destroy(instantiatedEventSystem);
    }

    public void _ContinuePressed()
    {
        Zelda._Game._GameMenuManager._Hide();
    }

    public void _MainMenuPressed()
    {
        Zelda._Common._SceneManager.ChangeScene(SceneManager.ESceneName.MainMenu, null);
    }
}
