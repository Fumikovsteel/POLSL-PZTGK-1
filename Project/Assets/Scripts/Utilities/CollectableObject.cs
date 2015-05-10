using UnityEngine;
using System.Collections;

public class CollectableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject collectableObject;

    private void OnTriggerEnter(Collider collider)
    {
        CollectorObject collector = collider.GetComponent<CollectorObject>();
        if (collector != null)
            collector._InvokeOnCollectEvent(collectableObject.GetComponent<ICollectableObject>());
    }
}
