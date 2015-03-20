using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputManager : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Structs

    public struct KeyData
    {
        public KeyCode keyCode;
        public EKeyUseType keyType;
    }

    public struct InputKeyTaker
    {
        public Func<bool> _CanTakeInput;
        public Action<InputManager.InputData> _OnInputUsed;
    }

    public enum EKeyUseType
    {
        pressed, released, pressedAndReleased
    }

    public struct InputData
    {
        public KeyCode usedKey;
        public EKeyUseType usedKeyType;
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region Fields

    private KeyCode[] usedKeyCodes = new KeyCode[0];
    private Dictionary<KeyCode, Dictionary<EKeyUseType, List<InputKeyTaker>>> inputTakers = new Dictionary<KeyCode, Dictionary<EKeyUseType, List<InputKeyTaker>>>();

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private void RunInputTakers(KeyCode cUsedKey, EKeyUseType cKeyType)
    {
        if (!inputTakers.ContainsKey(cUsedKey) || !inputTakers[cUsedKey].ContainsKey(cKeyType))
            return;

        InputData curInputData = new InputData() { usedKey = cUsedKey, usedKeyType = cKeyType };
        foreach (InputKeyTaker keyTaker in inputTakers[cUsedKey][cKeyType])
        {
            if (keyTaker._CanTakeInput())
                keyTaker._OnInputUsed(curInputData);
        }
    }

    private void RegisterKeyType(KeyCode keyCode, EKeyUseType keyUsetype, InputKeyTaker keyTaker)
    {
        if (!inputTakers[keyCode].ContainsKey(keyUsetype))
            inputTakers[keyCode].Add(keyUsetype, new List<InputKeyTaker>() { keyTaker });
        else
            inputTakers[keyCode][keyUsetype].Add(keyTaker);
    }

    private void Update()
    {
        foreach (KeyCode key in usedKeyCodes)
        {
            if (Input.GetKeyDown(key))
                RunInputTakers(key, EKeyUseType.pressed);
            else if (Input.GetKeyUp(key))
                RunInputTakers(key, EKeyUseType.released);
        }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void RegisterOnInput(InputKeyTaker inputKeyTaker, params KeyData[] keys)
    {
        foreach (KeyData usedKey in keys)
        {
            // If we haven't registered given key
            if (!Array.Exists(usedKeyCodes, x => x == usedKey.keyCode))
            {
                Array.Resize(ref usedKeyCodes, usedKeyCodes.Length + 1);
                usedKeyCodes[usedKeyCodes.Length - 1] = usedKey.keyCode;
            }

            if (!inputTakers.ContainsKey(usedKey.keyCode))
                inputTakers.Add(usedKey.keyCode, new Dictionary<EKeyUseType, List<InputKeyTaker>>());

            if (usedKey.keyType == EKeyUseType.pressedAndReleased)
            {
                RegisterKeyType(usedKey.keyCode, EKeyUseType.pressed, inputKeyTaker);
                RegisterKeyType(usedKey.keyCode, EKeyUseType.released, inputKeyTaker);
            }
            else
                RegisterKeyType(usedKey.keyCode, usedKey.keyType, inputKeyTaker);
            
        }
    }

    #endregion
}
