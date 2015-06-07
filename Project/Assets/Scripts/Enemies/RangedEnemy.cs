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
		Zelda._Common._GameplayEvents._OnLocationChanged += OnLocationChanged;
	}

    private void OnDestroy()
    {
        if (Zelda._Common != null)
            Zelda._Common._GameplayEvents._OnLocationChanged -= OnLocationChanged;
    }
	
	// Update is called once per frame
	void Update () {
			float curTime = Time.time;
			timeElaspedSinceLastFire += curTime - lastUpdate;
			if (timeElaspedSinceLastFire >= fireCooldown_S) {
				FireProjectile ();
				timeElaspedSinceLastFire = 0;
			}
			lastUpdate = curTime;
	}

	private void FireProjectile() {
		Transform playerTransform = Zelda._Game._GameManager._Player.transform;
		var distance = Vector3.Distance(playerTransform.position, transform.position);
		if (distance < range) {
			GameObject newProjectile = (GameObject) Instantiate(projectile, transform.position, transform.rotation);
			Rigidbody rb = (newProjectile).GetComponent<Rigidbody>();
			Physics.IgnoreCollision(GetComponent<Collider>(), rb.gameObject.GetComponent<Collider>());
            // We add some random to projectile direction
            Vector3 strikeVector = playerTransform.position - transform.position +
                                   new Vector3(UnityEngine.Random.Range(-0.35f, 0.35f), UnityEngine.Random.Range(-0.35f, 0.35f), 0.0f);
			strikeVector.Normalize ();
			rb.AddForce(strikeVector*shootForce);
			float angle = Mathf.Atan2(strikeVector.y, strikeVector.x) * Mathf.Rad2Deg - 90;
			newProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			rb.transform.SetParent(projectileParent,false);
		}
	}

	public void  OnLocationChanged() {
		enabled = !enabled;
	}
	
	public void _StopFiring()
    {
		enabled = false;
	}
	
	public void _UnstopFiring()
    {
		enabled = true;
	}
}
