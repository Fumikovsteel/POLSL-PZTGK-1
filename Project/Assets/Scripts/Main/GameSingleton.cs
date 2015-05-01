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
    private GameInitLevelData initData = null;
    public GameInitLevelData _InitData
    {
        get
        {
            if (initData == null)
                initData = Zelda._Common.GetInitLevelData(typeof(GameInitLevelData).ToString()) as GameInitLevelData;
            return initData;
        }
        private set { initData = value; }
    }
    private LevelInitLevelData levelInitData = null;
    public LevelInitLevelData _LevelInitData
    {
        get
        {
            if (levelInitData == null)
                levelInitData = Zelda._Common.GetInitLevelData(typeof(LevelInitLevelData).ToString()) as LevelInitLevelData;
            return levelInitData;
        }
        private set { levelInitData = value; }
    }
    private LocationInitLevelData locationInitData = null;
    public LocationInitLevelData _LocationInitData
    {
        get
        {
            if (locationInitData == null)
                locationInitData = Zelda._Common.GetInitLevelData(typeof(LocationInitLevelData).ToString()) as LocationInitLevelData;
            return locationInitData;
        }
        private set { locationInitData = value; }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InitializationMethods

    public GameSingleton()
    {
        Zelda._Common._GameplayEvents._OnSceneWillChange += OnSceneWillChange;
        Zelda._Common._GameplayEvents._OnLevelWillChange += OnLevelWillChange;
        Zelda._Common._GameplayEvents._OnLocationWillChange += OnLocationWillChange;
    }

    ~GameSingleton()
    {
        if (Zelda._Common != null)
        {
            Zelda._Common._GameplayEvents._OnSceneWillChange -= OnSceneWillChange;
            Zelda._Common._GameplayEvents._OnLevelWillChange -= OnLevelWillChange;
            Zelda._Common._GameplayEvents._OnLocationWillChange -= OnLocationWillChange;
        }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private void OnSceneWillChange(SceneManager.ESceneName sceneName)
    {
        _InitData = null;
        _LevelInitData = null;
        _LocationInitData = null;
    }

    private void OnLevelWillChange(LevelsManager.ELevelName levelName)
    {
        _LevelInitData = null;
        _LocationInitData = null;
    }

    private void OnLocationWillChange(LevelsManager.ELocationName locationName)
    {
        _LocationInitData = null;
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void Init()
    {
        _InputManager = (new GameObject("InputManager", typeof(InputManager))).GetComponent<InputManager>();
        _InputManager.transform.SetParentResetLocal(Zelda._Common._ManagersParent);

        Zelda._Common._ResourcesManager.InstantiatePrefab(ResourcesManager.EPrefabName.GameCamera, null);
        // We manually add new camera
        Zelda._Common._CamerasManager.Reinit();

        _GameManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<GameManager>(ResourcesManager.EPrefabName.GameManager, Zelda._Common._ManagersParent);
        _GameMenuManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<GameMenuManager>(ResourcesManager.EPrefabName.GameMenuManager, Zelda._Common._ManagersParent);
        _EnemiesManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<EnemiesManager>(ResourcesManager.EPrefabName.EnemiesManager, Zelda._Common._ManagersParent);
    }

    #endregion
}
