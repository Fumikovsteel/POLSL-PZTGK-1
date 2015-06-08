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
    private float showHideWeaponTime = 0.2f;
    [SerializeField]
    private float hitTime = 0.3f;
    [SerializeField]
    private float hitDelay = 0.1f;

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
    private float checkRaycastStep = 44.0f;
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
        iTween.ValueTo(sword.parent.gameObject, iTween.Hash("from", 45.0f, "to", 135.0f, "time", hitTime,
                                                            "easetype", iTween.EaseType.easeInQuad, "onupdate", "UpdateSecondStep",
                                                            "onupdatetarget", gameObject, "oncomplete", "SecondStepComplete",
                                                            "oncompletetarget", gameObject));
    }

    private void UpdateSecondStep(float curValue)
    {
        if (lastRaycastValue + checkRaycastStep < curValue)
        {
            CheckSwordRaycast();
            lastRaycastValue = Mathf.Floor(curValue / checkRaycastStep) * checkRaycastStep;
        }

        sword.parent.localRotation = Quaternion.Euler(0.0f, 0.0f, curValue);
    }

    private void CheckSwordRaycast()
    {
        RaycastHit raycastHit;
        Vector3 direction = new Vector3(sword.position.x - gameObject.transform.position.x, sword.position.y - gameObject.transform.position.y, 0.0f);
        Physics.Raycast(gameObject.transform.position, direction, out raycastHit, raycastLength, LayerMask.GetMask("Enemies"));
        if (raycastHit.collider != null)
        {
            Enemy hittedEnemy = raycastHit.collider.GetComponent<Enemy>();
            // We don't want to hit the same enemy twice in one attack animation
            if (hittedEnemy != null && !hittedEnemies.Contains(hittedEnemy))
            {
                hittedEnemies.Add(hittedEnemy);
                _OnEnemyHit(hittedEnemy, curAttackStrength, curRecoild);
            }
        }
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (direction.normalized * raycastLength), Color.black, 60.0f);
    }

    private void SecondStepComplete()
    {
        hittedEnemies.Clear();
        // Thanks to that we're sure we'll check raycast in first step
        lastRaycastValue = 270.0f;
        sword.localRotation = Quaternion.Euler(sword.localRotation.eulerAngles.x + 180.0f, sword.localRotation.eulerAngles.y, sword.localRotation.eulerAngles.z);
        iTween.ValueTo(sword.parent.gameObject, iTween.Hash("from", 135.0f, "to", 45.0f, "time", hitTime, "delay", hitDelay,
                                                            "easetype", iTween.EaseType.easeInQuad, "onupdate", "UpdateThirdStep",
                                                            "onupdatetarget", gameObject, "oncomplete", "ThirdStepComplete",
                                                            "oncompletetarget", gameObject));
    }

    private void UpdateThirdStep(float curValue)
    {
        if (lastRaycastValue - checkRaycastStep > curValue)
        {
            CheckSwordRaycast();
            lastRaycastValue = Mathf.Floor(curValue / checkRaycastStep) * checkRaycastStep;
        }

        sword.parent.localRotation = Quaternion.Euler(0.0f, 0.0f, curValue);
    }

    private void ThirdStepComplete()
    {
        hittedEnemies.Clear();
        sword.localRotation = Quaternion.Euler(sword.localRotation.eulerAngles.x + 180.0f, sword.localRotation.eulerAngles.y, sword.localRotation.eulerAngles.z);
        iTween.ScaleTo(sword.gameObject, iTween.Hash("scale", new Vector3(defaultSwordScale, defaultSwordScale, sword.localScale.z), "time", showHideWeaponTime,
                                                     "islocal", true));
        iTween.MoveTo(sword.gameObject, iTween.Hash("position", new Vector3(defaultSwordPosition, 0.0f, 0.0f), "time", showHideWeaponTime,
                                                    "islocal", true, "oncomplete", "FourthStepComplete", "oncompletetarget", gameObject));
    }

    private void FourthStepComplete()
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
            iTween.ScaleTo(sword.gameObject, iTween.Hash("scale", new Vector3(maxSwordScale, maxSwordScale, sword.localScale.z), "time", showHideWeaponTime,
                                                         "islocal", true));
            iTween.MoveTo(sword.gameObject, iTween.Hash("position", new Vector3(-maxSwordPosition, 0.0f, 0.0f), "time", showHideWeaponTime,
                                                        "islocal", true, "oncomplete", "FirstStepComplete", "oncompletetarget", gameObject));
            isCurrentlyAttacking = true;

			Zelda._Common._SoundManager.PlaySound(SoundManager.SoundName.PlayerAttack);
        }
    }
}
