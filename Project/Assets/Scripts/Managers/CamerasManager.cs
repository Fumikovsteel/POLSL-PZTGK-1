using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamerasManager
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Structs

    public enum ECameraName
    {
        mainMenuCamera, gameCamera
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region Fields

    private Dictionary<ECameraName, ZeldaCamera> allCameras = new Dictionary<ECameraName, ZeldaCamera>();

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public void Reinit()
    {
        allCameras.Clear();

        foreach (Camera curCamera in Camera.allCameras)
        {
            ZeldaCamera zeldaCamera = curCamera.GetComponent<ZeldaCamera>();
            if (zeldaCamera == null)
                Debug.LogError("All cameras should have ZeldaCamera component!");
            allCameras.Add(zeldaCamera._CameraName, zeldaCamera);
        }
    }

    public ZeldaCamera GetCamera(ECameraName cameraName)
    {
        return allCameras[cameraName];
    }

    #endregion
}
