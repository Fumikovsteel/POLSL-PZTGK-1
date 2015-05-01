using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public int dmg = 25;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision col)
	{
		Destroy (gameObject);
		Player player = col.gameObject.GetComponent<Player> ();
		if (player != null) {
			player.TakeLife(dmg);
		}
	}
	
}
