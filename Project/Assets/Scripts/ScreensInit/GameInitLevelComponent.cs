using UnityEngine;
using System.Collections;

public class GameInitLevelComponent : MonoBehaviour, IInitLevelComponent
{
    public IInitLevelData _GetInitLevelData
    { get { return initLevelData; } }

    public GameInitLevelData initLevelData;
}
