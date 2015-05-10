using UnityEngine;
using System.Collections;

public abstract class EquipmentItem : MonoBehaviour, ICollectableObject
{
    [SerializeField]
    private GameObject collectableObject;

    public abstract EquipmentManager.EEquipmentItem _ItemName
    { get; }

    public void _Collect()
    {
        Destroy(collectableObject);
    }
}
