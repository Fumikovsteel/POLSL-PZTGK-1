using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    private bool locked = false;
    public bool _Locked
    {
        get { return locked; }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InitializationManager

    public void Awake()
    {
        Zelda._Game._InputManager.RegisterOnInput(new InputManager.InputKeyTaker() { _CanTakeInput = () => { return !_Locked; }, _OnInputUsed = OnInputUsed },
                                                  new InputManager.KeyData() { keyCode = KeyCode.W, keyType = InputManager.EKeyUseType.pressedAndReleased },
                                                  new InputManager.KeyData() { keyCode = KeyCode.S, keyType = InputManager.EKeyUseType.pressedAndReleased },
                                                  new InputManager.KeyData() { keyCode = KeyCode.A, keyType = InputManager.EKeyUseType.pressedAndReleased },
                                                  new InputManager.KeyData() { keyCode = KeyCode.D, keyType = InputManager.EKeyUseType.pressedAndReleased });
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private void OnInputUsed(InputManager.InputData inputData)
    {
        Debug.Log(string.Format("Pressed key: {0} with type {1}", inputData.usedKey, inputData.usedKeyType));
    }

    #endregion
}
