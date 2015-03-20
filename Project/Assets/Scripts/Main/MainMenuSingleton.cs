using UnityEngine;
using System.Collections;

public class MainMenuSingleton
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    public MainMenuManager _MainMenuManager;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void Init()
    {
        _MainMenuManager = Zelda._Common._ResourcesManager.GetAndInstantiatePrefab<MainMenuManager>(ResourcesManager.EPrefabName.MainMenuManager, null);
    }

    #endregion
}
