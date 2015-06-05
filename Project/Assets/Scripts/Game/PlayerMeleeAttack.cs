using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField]
    private float maxSwordScale = 2.0f;
    [SerializeField]
    private float maxSwordPosition = 0.15f;
    [SerializeField]
    private float firstStepTime = 0.3f;
    [SerializeField]
    private float secondStepTime = 0.6f;
    [SerializeField]
    private float thirdStepTime = 0.3f;

    private Transform sword;
    private float defaultSwordScale;
    private float defaultSwordPosition;
    /// <summary>
    /// If we're in attack animation
    /// </summary>
    private bool isCurrentlyAttacking = false;
    /// <summary>
    /// On which angle we check raycast last time (help method for "UpdateSecondStep")
    /// </summary>
    private float lastRaycastValue = 0.0f;
    /// <summary>
    /// How often we will check raycast during sword rotating
    /// </summary>
    private float checkRaycastStep = 59.0f;
    /// <summary>
    /// How long sword raycast is
    /// </summary>
    private float raycastLength = 0.35f;
    /// <summary>
    /// All enemies which were hitted in current attack animation
    /// </summary>
    private List<Enemy> hittedEnemies = new List<Enemy>();

    /// <summary>
    /// Strength of the currently played attack
    /// </summary>
    private float curAttackStrength;
    /// <summary>
    /// Recoil of the currently player attack
    /// </summary>
    private float curRecoild;

    private void FirstStepComplete()
    {
        // Thanks to that we're sure we'll check raycast in first step
        lastRaycastValue = -180.0f;
        iTween.ValueTo(sword.parent.gameObject, iTween.Hash("from", 0.0f, "to", 180.0f, "time", secondStepTime,
                                                            "easetype", iTween.EaseType.easeInQuad, "onupdate", "UpdateSecondStep",
                                                            "onupdatetarget", gameObject, "oncomplete", "SecondStepComplete",
                                                            "oncompletetarget", gameObject));
    }

    private void UpdateSecondStep(float curValue)
    {
        RaycastHit raycastHit;
        if (lastRaycastValue + checkRaycastStep < curValue)
        {
            Vector3 direction = new Vector3(sword.position.x - gameObject.transform.position.x, sword.position.y - gameObject.transform.position.y, 0.0f);
            Physics.Raycast(gameObject.transform.position, direction, out raycastHit, raycastLength, LayerMask.GetMask("Enemies"));
            if (raycastHit.collider != null )
            {
                Enemy hittedEnemy = raycastHit.collider.GetComponent<Enemy>();
                // We don't want to hit the same enemy twice in one attack animation
                if (hittedEnemy != null && !hittedEnemies.Contains(hittedEnemy))
                {
                    hittedEnemies.Add(hittedEnemy);
                    _OnEnemyHit(hittedEnemy, curAttackStrength, curRecoild);
                }
            }
            lastRaycastValue = Mathf.Floor(curValue / checkRaycastStep) * checkRaycastStep;
            Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (direction.normalized * raycastLength), Color.black, 60.0f);
        }

        sword.parent.localRotation = Quaternion.Euler(0.0f, 0.0f, curValue);
    }

    private void SecondStepComplete()
    {
        iTween.ScaleTo(sword.gameObject, iTween.Hash("scale", new Vector3(defaultSwordScale, defaultSwordScale, sword.localScale.z), "time", thirdStepTime,
                                                     "islocal", true));
        iTween.ScaleTo(sword.parent.gameObject, iTween.Hash("scale", new Vector3(0.4f, 1.0f, 1.0f), "time", thirdStepTime/2.0f, "islocal", true,
                                                            "easetype", iTween.EaseType.linear, "oncomplete", "ThirdMidstepComplete",
                                                            "oncompletetarget", gameObject));
        iTween.MoveTo(sword.gameObject, iTween.Hash("position", new Vector3(defaultSwordPosition, sword.localPosition.y, sword.localPosition.z),
                                                    "time", thirdStepTime, "islocal", true, "oncomplete", "ThirdStepComplete",
                                                    "oncompletetarget", gameObject));
    }

    private void ThirdMidstepComplete()
    {
        hittedEnemies.Clear();
        sword.parent.localRotation = Quaternion.identity;
        iTween.ScaleTo(sword.parent.gameObject, iTween.Hash("scale", Vector3.one, "time", thirdStepTime / 2.0f, "islocal", true,
                                                            "easetype", iTween.EaseType.linear));
    }

    private void ThirdStepComplete()
    {
        isCurrentlyAttacking = false;
    }

    public event Action<Enemy, float, float> _OnEnemyHit = (x, y, z) => { };

    public void _StartAttackAnimation(Transform sword, float attackStrength, float attackRecoil)
    {
        if (!isCurrentlyAttacking)
        {
            curAttackStrength = attackStrength;
            curRecoild = attackRecoil;
            this.sword = sword;
            defaultSwordScale = sword.localScale.x;
            defaultSwordPosition = sword.localPosition.x;
            iTween.ScaleTo(sword.gameObject, iTween.Hash("scale", new Vector3(maxSwordScale, maxSwordScale, sword.localScale.z), "time", firstStepTime,
                                                         "islocal", true));
            iTween.MoveTo(sword.gameObject, iTween.Hash("position", new Vector3(-maxSwordPosition, 0.0f, 0.0f), "time", firstStepTime,
                                                        "islocal", true, "oncomplete", "FirstStepComplete", "oncompletetarget", gameObject));
            isCurrentlyAttacking = true;
        }
    }
}
