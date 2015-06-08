using UnityEngine;
using System.Collections;
using System;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCManager.ENPCName npcName;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite lookDownSprite;
    [SerializeField]
    private Sprite lookRightSprite;
    [SerializeField]
    private Sprite lookUpSprite;
    [SerializeField]
    private Sprite lookLeftSprite;

    private NPCManager npcManager;

    private void Awake()
    {
        _ChangeLookDirection(0.0f);
    }

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

    public void _ChangeLookDirection(float targetLookDirection)
    {
        if (targetLookDirection >= 359.0f || targetLookDirection <= 1.0f)
            spriteRenderer.sprite = lookDownSprite;
        else if (targetLookDirection >= 89.0f && targetLookDirection <= 91.0f)
            spriteRenderer.sprite = lookRightSprite;
        else if (targetLookDirection >= 179.0f && targetLookDirection <= 181.0f)
            spriteRenderer.sprite = lookUpSprite;
        else
            spriteRenderer.sprite = lookLeftSprite;
    }
}
