using UnityEngine;
using System.Collections;

public class ResourcesManager
{
    //////////////////////////////////////////////////////////////////////////////////
    #region Structs

    public enum EPrefabName
    {
        MainMenuManager, GameManager, GameMenuManager, EnemiesManager
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

    public T GetAndInstantiatePrefab<T>(EPrefabName prefabName, Transform newParent)
        where T:Component
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/" + prefabName.ToString());
        GameObject instantiatedPrefab = MonoBehaviour.Instantiate(prefab);
        if (newParent != null)
            instantiatedPrefab.transform.SetParentResetLocal(newParent);
        return instantiatedPrefab.GetComponent<T>();
    }

    #endregion
}
