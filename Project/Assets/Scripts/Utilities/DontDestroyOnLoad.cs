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
        Zelda._Common._GameplayEvents._OnLevelWillChange += OnLevelWillChange;
    }

    private void OnDestroy()
    {
        if (Zelda._Common)
            Zelda._Common._GameplayEvents._OnLevelWillChange -= OnLevelWillChange;
    }

    private void OnLevelWillChange()
    {
        if (_ShouldUnparentOnSceneChange)
        {
            List<Transform> allChilds = new List<Transform>();
            // Take references to all childre
            foreach (Transform child in transform)
                allChilds.Add(child);
            // Zeroes it's parent (object will be destroyed)
            foreach (Transform child in allChilds)
                child.parent = null;
        }
    }
}
