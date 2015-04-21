using UnityEngine;
using System.Collections;

public class GameSingleton
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    public InputManager _InputManager;
    public GameManager _GameManager;
    public GameMenuManager _GameMenuManager;
    public EnemiesManager _EnemiesManager;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void Init()
    {
        _InputManager = (new GameObject("InputManager", typeof(InputManager))).GetComponent<InputManager>();
        _InputManager.transform.SetParentResetLocal(Zelda._Common._ManagersParent);
        _GameManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<GameManager>(ResourcesManager.EPrefabName.GameManager, Zelda._Common._ManagersParent);
        _GameMenuManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<GameMenuManager>(ResourcesManager.EPrefabName.GameMenuManager, Zelda._Common._ManagersParent);
        _EnemiesManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<EnemiesManager>(ResourcesManager.EPrefabName.EnemiesManager, Zelda._Common._ManagersParent);
    }

    #endregion
}
