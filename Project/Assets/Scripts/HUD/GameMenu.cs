using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject eventSystem;
    [SerializeField]
    private GameObject continueButton;
    [SerializeField]
    private GameObject gameOverText;

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

    public void _Init(bool isGameOverScreen)
    {
        continueButton.SetActive(isGameOverScreen ? false : true);
        gameOverText.SetActive(isGameOverScreen ? true : false);
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
