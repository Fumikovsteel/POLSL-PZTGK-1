using UnityEngine;
using System.Collections;

public class ResourcesManager
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Structs

    public enum EPrefabName
    {
        MainMenuManager, GameManager, GameMenuManager
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public T GetAndInstantiatePrefab<T>(EPrefabName prefabName, Transform newParent)
        where T:Component
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/" + prefabName.ToString());
        GameObject instantiatedPrefab = MonoBehaviour.Instantiate(prefab);
        instantiatedPrefab.transform.SetParent(newParent, false);
        return instantiatedPrefab.GetComponent<T>();
    }

    #endregion
}
