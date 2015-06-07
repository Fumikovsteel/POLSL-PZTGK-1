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

    public NPCManager.ENPCName _Name
    { get { return npcName; } }

    public void _Init(NPCManager npcManager)
    {
        this.npcManager = npcManager;
    }
}
