﻿using UnityEngine;
using System.Collections;

public class ResourcesManager
{
	//////////////////////////////////////////////////////////////////////////////////
    #region Structs

	public enum EPrefabName
	{
		MainMenuManager,
		GameManager,
		GameMenuManager,
		EnemiesManager,
		GameCamera,
		HUDManager,
		CreditsManager,
		SoundManager,
		MusicManager,
		NPCManager,
		DialogueManager
	}

    #endregion
	//////////////////////////////////////////////////////////////////////////////////
    #region Fields

	private const string prefabsPrefix = "Prefabs/";

    #endregion
	//////////////////////////////////////////////////////////////////////////////////
    #region InsideMethods

	public GameObject InstantiateObject (string path, Transform newParent)
	{
		GameObject prefab = (GameObject)Resources.Load (path);
		GameObject instantiatedPrefab = MonoBehaviour.Instantiate (prefab);
		if (newParent != null)
			instantiatedPrefab.transform.SetParentResetLocal (newParent);
		return instantiatedPrefab;
	}

    #endregion
	//////////////////////////////////////////////////////////////////////////////////
    #region OutsideMethods

	public T GetAndInstantiatePrefab<T> (EPrefabName prefabName, Transform newParent)
        where T:Component
	{
		return InstantiatePrefab (prefabName, newParent).GetComponent<T> ();
	}

	public GameObject InstantiatePrefab (EPrefabName prefabName, Transform newParent)
	{
		return InstantiateObject (prefabsPrefix + prefabName.ToString (), newParent);
	}

    #endregion
}
