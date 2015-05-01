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
    public event Action _OnLocationWillChange = () => { };
    public event Action _OnLocationChanged = () => { };

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

    public void RaiseOnLocationWillChange()
    {
        _OnLocationWillChange();
    }

    public void RaiseOnLocationChanged()
    {
        _OnLocationChanged();
    }

    #endregion
}
