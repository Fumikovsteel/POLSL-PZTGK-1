using UnityEngine;
using System.Collections;
using System;

public class GameplayEvents
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    public event Action _OnLevelWasLoaded = () => { };
    public event Action<SceneManager.ESceneName> _OnSceneWillChange = (x) => { };
    public event Action<LevelsManager.ELevelName> _OnLevelWillChange = (x) => { };
    public event Action<LevelsManager.ELocationName> _OnLocationWillChange = (x) => { };
    public event Action _OnLocationChanged = () => { };
    public event Action _OnGamePaused = () => { };
    public event Action _OnGameUnpaused = () => { };
    public event Action _OnPlayerKilled = () => { };
    public event Action _OnPlayerWillBeKilled = () => { };

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void RaiseOnLevelWasLoaded()
    {
        _OnLevelWasLoaded();
    }

    public void RaiseOnSceneWillChange(SceneManager.ESceneName newScene)
    {
        _OnSceneWillChange(newScene);
    }

    public void RaiseOnLevelWillChange(LevelsManager.ELevelName newLevel)
    {
        _OnLevelWillChange(newLevel);
    }

    public void RaiseOnLocationWillChange(LevelsManager.ELocationName newLocation)
    {
        _OnLocationWillChange(newLocation);
    }

    public void RaiseOnLocationChanged()
    {
        _OnLocationChanged();
    }

    public void RaiseOnGamePaused()
    {
        _OnGamePaused();
    }

    public void RaiseOnGameUnpaused()
    {
        _OnGameUnpaused();
    }

    public void RaiseOnPlayerKilled()
    {
        _OnPlayerKilled();
    }

    public void RaiseOnPlayerWillBeKilled()
    {
        _OnPlayerWillBeKilled();
    }

    #endregion
}
