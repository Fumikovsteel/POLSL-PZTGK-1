using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

public class CollectorObject : MonoBehaviour
{
    [Serializable]
    public class OnCollectEvent : UnityEvent<ICollectableObject>
    { }

    [SerializeField]
    private OnCollectEvent onCollectEvent = new OnCollectEvent();

    public void _InvokeOnCollectEvent(ICollectableObject collectedObject)
    {
        onCollectEvent.Invoke(collectedObject);
    }
}
