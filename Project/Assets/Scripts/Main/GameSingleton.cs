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
    public GameInitLevelData _InitData;
    private LevelInitLevelData levelInitData = null;
    public LevelInitLevelData _LevelInitData
    {
        get
        {
            if (levelInitData == null)
                levelInitData = Zelda._Common._InitLevelData as LevelInitLevelData;
            return levelInitData;
        }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private void OnLevelWillChange(LevelsManager.ELevelName levelName)
    {
        levelInitData = Zelda._Common._InitLevelData as LevelInitLevelData;
    }

    private void OnDestroy()
    {
        if (Zelda._Common != null)
            Zelda._Common._GameplayEvents._OnLevelWillChange -= OnLevelWillChange;
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void Init()
    {
        _InitData = Zelda._Common._InitLevelData as GameInitLevelData;

        _InputManager = (new GameObject("InputManager", typeof(InputManager))).GetComponent<InputManager>();
        _InputManager.transform.SetParentResetLocal(Zelda._Common._ManagersParent);

        Zelda._Common._ResourcesManager.InstantiatePrefab(ResourcesManager.EPrefabName.GameCamera, null);
        // We manually add new camera
        Zelda._Common._CamerasManager.Reinit();

        _GameManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<GameManager>(ResourcesManager.EPrefabName.GameManager, Zelda._Common._ManagersParent);
        _GameMenuManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<GameMenuManager>(ResourcesManager.EPrefabName.GameMenuManager, Zelda._Common._ManagersParent);
        _EnemiesManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<EnemiesManager>(ResourcesManager.EPrefabName.EnemiesManager, Zelda._Common._ManagersParent);

        Zelda._Common._GameplayEvents._OnLevelWillChange += OnLevelWillChange;
    }

    #endregion
}
