using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public int dmg = 25;
	public float projectileLifetime_S = 5;

	private static Transform projectileParent;

	private Rigidbody rb;
	private Vector3 savedVelocity;

	// Use this for initialization
	void Start () {
		Invoke ("DestroyProjectile", projectileLifetime_S);
		if (Projectile.projectileParent == null) {
			projectileParent = new GameObject ("ProjectileParent", typeof(DontDestroyOnLoad)).transform;
		}
		transform.SetParent(projectileParent, false);
		Zelda._Common._GameplayEvents._OnLocationChanged += OnLocationChanged;
		Zelda._Common._GameplayEvents._OnGamePaused += OnGamePaused;
		Zelda._Common._GameplayEvents._OnGameUnpaused += OnGameUnpaused;
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision col)
	{
		DestroyProjectile ();
		if(col.gameObject.tag.Equals("Player")) {
			Zelda._Game._GameManager._Player.TakeLife(dmg);
		}
	}

	private void DestroyProjectile() {
		Destroy (gameObject);
		Zelda._Common._GameplayEvents._OnLocationChanged -= OnLocationChanged;
		Zelda._Common._GameplayEvents._OnGamePaused -= OnGamePaused;
		Zelda._Common._GameplayEvents._OnGameUnpaused -= OnGameUnpaused;

	}

	public void  OnLocationChanged() {
		DestroyProjectile ();
	}
	
	public void OnGamePaused() {
		savedVelocity = rb.velocity;
		rb.isKinematic = true;
		enabled = false;
	}
	
	public void OnGameUnpaused() {
		rb.isKinematic = false;
		rb.AddForce( savedVelocity, ForceMode.VelocityChange );
		enabled = true;
	}
	
}
