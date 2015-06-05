using UnityEngine;
using System.Collections;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameMenuPrefab;

    private bool Locked;
    private GameObject instantiatedGameMenu = null;
    
    private void Awake()
    {
        Zelda._Game._InputManager.RegisterOnInput(new InputManager.InputKeyTaker() { _CanTakeInput = () => { return !Locked; }, _OnInputUsed = OnInputUsed },
                                                  new InputManager.KeyData() { keyCode = KeyCode.Escape, keyType = InputManager.EKeyUseType.released });
        Zelda._Common._GameplayEvents._OnPlayerKilled += ShowGameOver;
    }

    private void OnDestroy()
    {
        if (Zelda._Common != null)
            Zelda._Common._GameplayEvents._OnPlayerKilled -= ShowGameOver;
    }

    private void ShowGameOver()
    {
        CreateGameMenu(true);
        Locked = true;
    }

    private void OnInputUsed(InputManager.InputData inputData)
    {
        if (instantiatedGameMenu == null)
            CreateGameMenu(false);
        else
            _Hide();
    }

    private void CreateGameMenu(bool isGameOverScreen)
    {
        Zelda._Common._GameplayEvents.RaiseOnGamePaused();
        instantiatedGameMenu = Instantiate(gameMenuPrefab) as GameObject;
        instantiatedGameMenu.GetComponent<GameMenu>()._Init(isGameOverScreen);
    }

    public void _Hide()
    {
        Zelda._Common._GameplayEvents.RaiseOnGameUnpaused();
        Destroy(instantiatedGameMenu);
        instantiatedGameMenu = null;
    }
}
