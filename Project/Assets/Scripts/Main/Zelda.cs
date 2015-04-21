using UnityEngine;
using System.Collections;

public class Zelda
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Structs

    public enum ESingletonState
    {
        noState, initialized, noAccess,
        // We want to initialize singleton one more time
        forceInitialize
    }

    public enum ESingletonName
    {
        empty, common, game, mainMenu
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    private static ESingletonState mainMenuState = ESingletonState.noState;
    private static MainMenuSingleton mainMenu;
    public static MainMenuSingleton _MainMenu
    {
        get
        {
            if (CommonSingleton.CanAccess())
            {
                if (mainMenuState == ESingletonState.noState || mainMenuState == ESingletonState.forceInitialize)
                {
                    mainMenu = new MainMenuSingleton();
                    mainMenuState = ESingletonState.initialized;
                    mainMenu.Init();
                }
                if (mainMenuState == ESingletonState.initialized)
                    return mainMenu;
                else if (mainMenuState == ESingletonState.noAccess)
                    Debug.Log("You have no access to MainMenu singleton!");
            }
            return null;
        }
        private set { mainMenu = value; }
    }

    private static ESingletonState gameState = ESingletonState.noState;
    private static GameSingleton game;
    public static GameSingleton _Game
    {
        get
        {
            if (CommonSingleton.CanAccess())
            {
                if (gameState == ESingletonState.noState || gameState == ESingletonState.forceInitialize)
                {
                    game = new GameSingleton();
                    gameState = ESingletonState.initialized;
                    game.Init();
                }
                if (gameState == ESingletonState.initialized)
                    return game;
                else if (gameState == ESingletonState.noAccess)
                    Debug.Log("You have no access to Game singleton!");
            }
            return null;
        }
        private set { game = value; }
    }

    private static ESingletonState commonState = ESingletonState.noState;
    private static CommonSingleton common;
    public static CommonSingleton _Common
    {
        get
        {
            if (CommonSingleton.CanAccess())
            {
                if (commonState == ESingletonState.noState)
                {
                    common = new GameObject("Zelda", typeof(CommonSingleton), typeof(DontDestroyOnLoad)).GetComponent<CommonSingleton>();
                    commonState = ESingletonState.initialized;
                    common.Init();
                }
                if (commonState == ESingletonState.initialized)
                    return common;
                else if (commonState == ESingletonState.noAccess)
                    Debug.Log("You don't have access to Common singleton!");
            }
            return null;
        }
        private set { common = value; }
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private static bool AbleToChangeState(ESingletonState curState, ESingletonState newState)
    {
        return !((curState == newState) || (curState == ESingletonState.initialized && newState == ESingletonState.forceInitialize));
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public static void ChangeState(ESingletonName singleton, ESingletonState newState)
    {
        switch (singleton)
        {
            case ESingletonName.mainMenu:
                if (AbleToChangeState(mainMenuState, newState))
                    mainMenuState = newState;
                break;
            case ESingletonName.game:
                if (AbleToChangeState(gameState, newState))
                    gameState = newState;
                break;
            case ESingletonName.common:
                if (AbleToChangeState(commonState, newState))
                    commonState = newState;
                break;
            default:
                Debug.LogError("Given singleton not supported!"); break;
        }
    }

    public static void Init(ESingletonName singleton)
    {
        switch (singleton)
        {
            case ESingletonName.mainMenu:
                _MainMenu = _MainMenu; break;
            case ESingletonName.game:
                _Game = _Game; break;
            case ESingletonName.common:
                _Common = _Common; break;
            default:
                Debug.LogError("Given singleton not supported!"); break;
        }
    }

    #endregion
}
