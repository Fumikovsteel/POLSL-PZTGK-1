using UnityEngine;
using System.Collections;

public class PlayerMeleeAttack : MonoBehaviour {

	public float attackAngle = 45;
	public float attackRange = 0.1f;


	LayerMask enemyLayerMask = LayerMask.GetMask("Enemies");
	// Use this for initialization

	public void Attack() {
		Collider[] collidersInRange = Physics.OverlapSphere (transform.position, attackRange, enemyLayerMask);
		Vector3 playerDirection = Zelda._Game._GameManager._Player._PlayerDirection;
		Vector3 playerPosition = Zelda._Game._GameManager._Player.transform.position;

		foreach(Collider c in collidersInRange) {
			Vector3 playerToEnemyDirection = c.transform.position - playerPosition;
			float angle = Vector3.Angle(playerDirection, playerToEnemyDirection);
			float distance = Vector3.Distance(playerPosition, c.transform.position);
			if(distance <= attackRange && (angle > - attackAngle/2 && angle < attackAngle/2)) {
				Destroy(c.gameObject);
			}
		}

	}


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
