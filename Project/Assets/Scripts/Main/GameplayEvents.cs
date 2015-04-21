using UnityEngine;
using System.Collections;
using System;

public class GameplayEvents
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    public event Action _OnLevelWasLoaded = () => {};
    public event Action _OnLevelWillChange = () => { };

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void RaiseOnLevelWasLoaded()
    {
        _OnLevelWasLoaded();
    }

    public void RaiseOnLevelWillChange()
    {
        _OnLevelWillChange();
    }

    #endregion
}
