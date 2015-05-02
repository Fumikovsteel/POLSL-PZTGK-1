using UnityEngine;
using System.Collections;

public class ResourcesManager
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Structs

    public enum EPrefabName
    {
        MainMenuManager, GameManager, GameMenuManager, EnemiesManager, GameCamera, HUDManager
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region Fields

    private const string prefabsPrefix = "Prefabs/";

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public T GetAndInstantiatePrefab<T>(EPrefabName prefabName, Transform newParent)
        where T:Component
    {
        return InstantiatePrefab(prefabName, newParent).GetComponent<T>();
    }

    public GameObject InstantiatePrefab(EPrefabName prefabName, Transform newParent)
    {
        GameObject prefab = (GameObject)Resources.Load(prefabsPrefix + prefabName.ToString());
        GameObject instantiatedPrefab = MonoBehaviour.Instantiate(prefab);
        if (newParent != null)
            instantiatedPrefab.transform.SetParentResetLocal(newParent);
        return instantiatedPrefab;
    }

    #endregion
}
