﻿using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////
    #region InitializationMethods

    private void Awake()
    {
        // First thing which happen during reloading scene
        if (!Zelda._Common._SceneManager._FirstScene)
            Zelda._Common._SceneManager.OnNewSceneLoaded();

        Zelda._Common._CamerasManager.Reinit();

        // We make sure that common singleton has been initialized
        Zelda.Init(Zelda.ESingletonName.common);
#if UNITY_EDITOR
        if (Zelda._Common.GetInitLevelDataCount() <= 0)
        {
            IInitLevelComponent[] initComponents = GetComponents<IInitLevelComponent>();
            foreach (IInitLevelComponent component in initComponents)
                Zelda._Common.ChangeInitLevelData(component._GetInitLevelData);
        }
#endif
        Zelda.ESingletonName sceneSingleton = Zelda._Common._SceneManager._CurSceneSingleton;
        if (sceneSingleton != Zelda.ESingletonName.empty)
            Zelda.Init(sceneSingleton);

        if (Zelda._Common._SceneManager._FirstScene)
            Zelda._Common._GameplayEvents.RaiseOnLevelWasLoaded();
        Destroy(gameObject);
    }

    #endregion
}
