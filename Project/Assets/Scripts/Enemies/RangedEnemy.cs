using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour {

	public float range;
	public GameObject projectile;
	public float fireCooldown_S;
	public int shootForce = 500;

	private float timeElaspedSinceLastFire;
	private float lastUpdate;
	private Transform projectileParent; 

	// Use this for initialization
	void Start () {
		timeElaspedSinceLastFire = fireCooldown_S;
		lastUpdate = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float curTime = Time.time;
		timeElaspedSinceLastFire += curTime - lastUpdate;
		if (timeElaspedSinceLastFire >= fireCooldown_S) {
			FireProjectile();
			timeElaspedSinceLastFire = 0;
		}
		lastUpdate = curTime;
	}

	private void FireProjectile() {
		Transform playerTransform = Zelda._Game._GameManager._Player.transform;
		var distance = Vector3.Distance(playerTransform.position, transform.position);
		if (distance < range) {
			Rigidbody rb = ((GameObject) Instantiate(projectile, transform.position, transform.rotation)).GetComponent<Rigidbody>();
			Physics.IgnoreCollision(GetComponent<Collider>(), rb.gameObject.GetComponent<Collider>());
			Vector3 strikeVector = playerTransform.position - transform.position;
			strikeVector.Normalize ();
			rb.AddForce(strikeVector*shootForce);
			rb.transform.SetParent(projectileParent,false);
		}
	}
}
