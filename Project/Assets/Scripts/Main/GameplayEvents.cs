using UnityEngine;
using System.Collections;
using System;

public class GameplayEvents
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    public event Action _OnLevelWasLoaded = () => {};

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void RaiseOnLevelWasLoaded()
    {
        _OnLevelWasLoaded();
    }

    #endregion
}
