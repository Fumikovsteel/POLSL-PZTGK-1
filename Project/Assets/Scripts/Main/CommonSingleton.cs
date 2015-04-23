using UnityEngine;
using System.Collections;

public class CommonSingleton : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    public SceneManager _SceneManager;
    public IInitLevelData _InitLevelData;
    public GameplayEvents _GameplayEvents;
    public ResourcesManager _ResourcesManager;
    public CamerasManager _CamerasManager;
    public ApplicationManager _ApplicationManager;
    public Transform _ManagersParent;
    public LevelsManager _LevelsManager;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region Fields

    private DontDestroyOnLoad managersParentDontDestroy;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    /// <summary>
    /// This event is executed here first (because of the script execution order) so here we set application quitting flag
    /// </summary>
    private void OnApplicationQuit()
    {
        ApplicationManager._ApplicationIsQuitting = true;
    }

    private void OnLevelWasLoaded()
    {
        Zelda._Common._GameplayEvents.RaiseOnLevelWasLoaded();
    }

    private void OnSceneWillChange(SceneManager.ESceneName newScene)
    {
        managersParentDontDestroy._ShouldUnparentOnSceneChange = _SceneManager._CurScene != newScene ? true : false;
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public static bool CanAccess()
    {
        return !ApplicationManager._ApplicationIsQuitting;
    }

    public void Init()
    {
        _ManagersParent = new GameObject("Managers parent", typeof(DontDestroyOnLoad)).transform;
        managersParentDontDestroy = _ManagersParent.GetComponent<DontDestroyOnLoad>();

        _ApplicationManager = new ApplicationManager();
        _GameplayEvents = new GameplayEvents();
        _SceneManager = new SceneManager();
        _LevelsManager = new LevelsManager();
        _CamerasManager = new CamerasManager();
        _ResourcesManager = new ResourcesManager();

        _GameplayEvents._OnSceneWillChange += OnSceneWillChange;
    }

    #endregion
}
