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

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region Fields

    private static bool _ApplicationIsQuitting = false;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InitializationMethods

    public CommonSingleton()
    {
        DontDestroyOnLoad(this);
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private void OnApplicationQuit()
    {
        _ApplicationIsQuitting = true;
    }

    private void OnLevelWasLoaded()
    {
        Zelda._Common._GameplayEvents.RaiseOnLevelWasLoaded();
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public static bool CanAccess()
    {
        return !_ApplicationIsQuitting;
    }

    public void Init()
    {
        _GameplayEvents = new GameplayEvents();
        _SceneManager = new SceneManager();
        _CamerasManager = new CamerasManager();
        _ResourcesManager = new ResourcesManager();
    }

    #endregion
}
