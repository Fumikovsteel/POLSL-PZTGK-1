using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float speed = 3.0f;
	public float moveToDistance;

	private bool shouldMove = true;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Transform playerTransform = Zelda._Game._GameManager._Player.transform;
		var distance = Vector3.Distance(playerTransform.position, transform.position);
		if (distance >= moveToDistance) {
			shouldMove = true;
		} else {
			shouldMove = false;
		}
		MoveTowardsPlayer ();
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetComponent<Player> () != null) {
			shouldMove = false;
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.GetComponent<Player> () != null) {
			shouldMove = true;
		}
	}

	private void MoveTowardsPlayer()
	{
		if (shouldMove) {
			Transform playerTransform = Zelda._Game._GameManager._Player.transform;
			Vector3 strikeVector = playerTransform.position - transform.position;
			strikeVector.Normalize ();
			rb.velocity = strikeVector * speed;
		} else {
			rb.velocity = Vector3.zero;
		}
	}
}
