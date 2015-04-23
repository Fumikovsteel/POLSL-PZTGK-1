using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////
    #region InspectorProperties

    public GameObject PlayerPrefab;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region Properties

    [HideInInspector]
    public Player _Player;
    [HideInInspector]
    public ZeldaCamera _GameCamera;

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InitializationMethods

    private void Awake()
    {
        _GameCamera = Zelda._Common._CamerasManager.GetCamera(CamerasManager.ECameraName.gameCamera);
        _Player = Instantiate(PlayerPrefab).GetComponent<Player>();

        Zelda._Common._GameplayEvents._OnSceneWillChange += OnSceneWillChange;
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

    private void OnSceneWillChange(SceneManager.ESceneName newScene)
    {
        // Because it has DontDestroyOnLoad
        if (newScene != SceneManager.ESceneName.Game)
            Destroy(_GameCamera);
    }

    private void OnDestroy()
    {
        if (Zelda._Common != null)
            Zelda._Common._GameplayEvents._OnSceneWillChange -= OnSceneWillChange;
    }

    #endregion
}
