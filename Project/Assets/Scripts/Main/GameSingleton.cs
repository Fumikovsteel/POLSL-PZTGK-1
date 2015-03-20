using UnityEngine;
using System.Collections;

public class GameSingleton
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    public InputManager _InputManager;
    public GameManager _GameManager;
    public GameMenuManager _GameMenuManager;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void Init()
    {
        _InputManager = (new GameObject("InputManager", typeof(InputManager))).GetComponent<InputManager>();
        _GameManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<GameManager>(ResourcesManager.EPrefabName.GameManager, null);
        _GameMenuManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<GameMenuManager>(ResourcesManager.EPrefabName.GameMenuManager, null);
    }

    #endregion
}
