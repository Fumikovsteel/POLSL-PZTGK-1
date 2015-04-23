using UnityEngine;
using System.Collections;

public class LevelInitLevelComponent : MonoBehaviour, IInitLevelComponent
{
    public IInitLevelData _GetInitLevelData
    {
        get { return levelInitData; }
    }

    [SerializeField]
    private LevelInitLevelData levelInitData;
}
