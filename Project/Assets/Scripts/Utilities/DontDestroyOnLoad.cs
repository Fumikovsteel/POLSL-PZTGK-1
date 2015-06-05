using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DontDestroyOnLoad : MonoBehaviour
{
    public bool _ShouldUnparentOnSceneChange = true;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Zelda._Common._GameplayEvents._OnSceneWillChange += OnSceneWillChange;
    }

    private void OnDestroy()
    {
        if (Zelda._Common)
            Zelda._Common._GameplayEvents._OnSceneWillChange -= OnSceneWillChange;
    }

    private void OnSceneWillChange(SceneManager.ESceneName newScene)
    {
        if (_ShouldUnparentOnSceneChange)
        {
            List<Transform> allChilds = new List<Transform>();
            // Take references to all childre
            foreach (Transform child in transform)
            {
                if (child.GetComponent<DontDestroyOnLoad>() == null)
                    allChilds.Add(child);
            }
            // Zeroes it's parent (object will be destroyed)
            foreach (Transform child in allChilds)
                child.parent = null;
        }
    }
}
