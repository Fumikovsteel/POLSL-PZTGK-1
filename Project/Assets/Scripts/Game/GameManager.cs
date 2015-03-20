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

    public void Awake()
    {
        _GameCamera = Zelda._Common._CamerasManager.GetCamera(CamerasManager.ECameraName.gameCamera);
        _Player = Instantiate(PlayerPrefab).GetComponent<Player>();
        _GameCamera.transform.SetParent(_Player.transform, false);
    }

    #endregion
}
