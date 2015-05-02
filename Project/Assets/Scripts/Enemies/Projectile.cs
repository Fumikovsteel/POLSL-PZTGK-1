using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public int dmg = 25;
	public float projectileLifetime_S = 5;

	private static Transform projectileParent;

	// Use this for initialization
	void Start () {
		Invoke ("DestroyProjectile", projectileLifetime_S);
		if (Projectile.projectileParent == null) {
			projectileParent = new GameObject ("ProjectileParent", typeof(DontDestroyOnLoad)).transform;
		}
		transform.SetParent(projectileParent, false);
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
	}
	
}
