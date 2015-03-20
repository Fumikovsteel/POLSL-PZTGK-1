using UnityEngine;
using System.Collections;

public class GameMenuManager : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    private bool locked = false;
    private bool _Locked
    {
        get { return locked; }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InitializationMethods

    private void Awake()
    {
        Zelda._Game._InputManager.RegisterOnInput(new InputManager.InputKeyTaker() { _CanTakeInput = () => { return !_Locked; }, _OnInputUsed = OnInputUsed },
                                                  new InputManager.KeyData() { keyCode = KeyCode.Escape, keyType = InputManager.EKeyUseType.released });
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private void OnInputUsed(InputManager.InputData inputData)
    {
        Zelda._Common._SceneManager.ChangeScene(SceneManager.ESceneName.MainMenu, null);
    }

    #endregion
}
