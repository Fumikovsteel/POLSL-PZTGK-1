using UnityEngine;
using System.Collections;

public class LocationInitLevelComponent : MonoBehaviour, IInitLevelComponent
{
    public IInitLevelData _GetInitLevelData
    {
        get { return locationInitData; }
    }

    [SerializeField]
    private LocationInitLevelData locationInitData;
}
