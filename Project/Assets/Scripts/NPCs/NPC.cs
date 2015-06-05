using UnityEngine;
using System.Collections;
using System;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCManager.ENPCName npcName;

    private NPCManager npcManager;

    private void OnTriggerEnter(Collider curCollider)
    {
        npcManager._ShowCanTalkMessage(this);
    }

    private void OnTriggerExit(Collider curCollider)
    {
        npcManager._HideCanTalkMessage();
    }

    private void OnRotateAnimationFinished(Action onAnimationFinished)
    {
        if (onAnimationFinished != null)
            onAnimationFinished();
    }

    public NPCManager.ENPCName _Name
    { get { return npcName; } }

    public void _Init(NPCManager npcManager)
    {
        this.npcManager = npcManager;
    }

    public void _RotateToPlayer(Vector3 targetRotation, float rotateTime, Action onRotateFinished)
    {
        iTween.RotateTo(gameObject, iTween.Hash("rotation", targetRotation, "time", rotateTime, "easetype", iTween.EaseType.linear,
                                                "oncomplete", "OnRotateAnimationFinished", "oncompleteparams", onRotateFinished));
    }
}
