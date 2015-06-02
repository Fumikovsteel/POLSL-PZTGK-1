using UnityEngine;
using System.Collections;

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
    private float checkRaycastStep = 50.0f;

    //public float attackAngle = 45;
    //public float attackRange = 0.1f;


    //LayerMask enemyLayerMask = LayerMask.GetMask("Enemies");
    //// Use this for initialization

    //public void Attack() {
    //    Collider[] collidersInRange = Physics.OverlapSphere (transform.position, attackRange, enemyLayerMask);
    //    Vector3 playerDirection = Zelda._Game._GameManager._Player._PlayerDirection;
    //    Vector3 playerPosition = Zelda._Game._GameManager._Player.transform.position;

    //    foreach(Collider c in collidersInRange) {
    //        Vector3 playerToEnemyDirection = c.transform.position - playerPosition;
    //        float angle = Vector3.Angle(playerDirection, playerToEnemyDirection);
    //        float distance = Vector3.Distance(playerPosition, c.transform.position);
    //        if(distance <= attackRange && (angle > - attackAngle/2 && angle < attackAngle/2)) {
    //            Destroy(c.gameObject);
    //        }
    //    }
    //}

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
        if (lastRaycastValue + checkRaycastStep < curValue)
        {
            RaycastHit raycastHit;
            Vector3 direction = new Vector3(sword.position.x - gameObject.transform.position.x, sword.position.y - gameObject.transform.position.y, 0.0f);
            Physics.Raycast(gameObject.transform.position, direction, out raycastHit, 2.0f, LayerMask.NameToLayer("Enemies"));
            if (raycastHit.collider != null)
                Debug.Log(direction);
            lastRaycastValue = curValue;
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
        sword.parent.localRotation = Quaternion.identity;
        iTween.ScaleTo(sword.parent.gameObject, iTween.Hash("scale", Vector3.one, "time", thirdStepTime / 2.0f, "islocal", true,
                                                            "easetype", iTween.EaseType.linear));
    }

    private void ThirdStepComplete()
    {
        isCurrentlyAttacking = false;
    }

    public void _StartAttackAnimation(Transform sword)
    {
        if (!isCurrentlyAttacking)
        {
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
