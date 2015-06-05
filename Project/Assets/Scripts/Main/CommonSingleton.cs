using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonSingleton : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    public SceneManager _SceneManager;
    public GameplayEvents _GameplayEvents;
    public ResourcesManager _ResourcesManager;
    public CamerasManager _CamerasManager;
    public ApplicationManager _ApplicationManager;
    public Transform _ManagersParent;
    public LevelsManager _LevelsManager;
    public TimeManager _TimeManager;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region Fields

    private DontDestroyOnLoad managersParentDontDestroy;
    public List<IInitLevelData> initLevelData = new List<IInitLevelData>();

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
        _TimeManager = new GameObject("TimeManager", typeof(TimeManager), typeof(DontDestroyOnLoad)).GetComponent<TimeManager>();

        _GameplayEvents._OnSceneWillChange += OnSceneWillChange;
    }

    public void ClearInitLevelData()
    {
        initLevelData.Clear();
    }

    public void ChangeInitLevelData(IInitLevelData newData)
    {
        for (int i = 0; i < initLevelData.Count; i++)
        {
            // We change old init value to new one
            if (initLevelData[i].GetType().ToString() == newData.GetType().ToString())
            {
                initLevelData[i] = newData;
                return;
            }
        }
        initLevelData.Add(newData);
    }

    public IInitLevelData GetInitLevelData(string initLevelDataTypeName)
    {
        foreach (IInitLevelData levelData in initLevelData)
        {
            if (levelData.GetType().ToString() == initLevelDataTypeName)
                return levelData;
        }
        return null;
    }

    public int GetInitLevelDataCount()
    {
        return initLevelData.Count;
    }

    #endregion
}
