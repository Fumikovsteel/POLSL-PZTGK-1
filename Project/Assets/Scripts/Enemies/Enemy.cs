using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RangedEnemy))]
public class Enemy : MonoBehaviour {

	public float speed = 3.0f;
	public float moveToDistance;
	private Vector3 previousLocation = new Vector3();

	private bool shouldMove = true;
	private Rigidbody rb;
	private Vector3 savedVelocity;
    private RangedEnemy rangedEnemyComponent;

	private Animator animator;

    private float curHealth = 0.0f;
    private int curDirection = 0;

    private void Awake()
    {
        rangedEnemyComponent = GetComponent<RangedEnemy>();
    }

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		animator = this.GetComponent<Animator>();
		Zelda._Common._GameplayEvents._OnLocationChanged += OnLocationChanged;
		Zelda._Common._GameplayEvents._OnGamePaused += OnGamePaused;
		Zelda._Common._GameplayEvents._OnGameUnpaused += OnGameUnpaused;
        Zelda._Common._GameplayEvents._OnPlayerWillBeKilled += _StopEnemyMovement;
	}

    private void OnDestroy()
    {
        if (Zelda._Common != null)
        {
            Zelda._Common._GameplayEvents._OnLocationChanged -= OnLocationChanged;
            Zelda._Common._GameplayEvents._OnGamePaused -= OnGamePaused;
            Zelda._Common._GameplayEvents._OnGameUnpaused -= OnGameUnpaused;
            Zelda._Common._GameplayEvents._OnPlayerWillBeKilled -= _StopEnemyMovement;
        }
    }
	
	// Update is called once per frame
	void Update () {
			Transform playerTransform = Zelda._Game._GameManager._Player.transform;
			var distance = Vector3.Distance (playerTransform.position, transform.position);
			if (distance >= moveToDistance) {
				shouldMove = true;
			} else {
				shouldMove = false;
			}
			MoveTowardsPlayer ();
		UpdateAnimations ();
	}

	private void UpdateAnimations()
	{
        Vector3 playersPosition = Zelda._Game._GameManager._Player._PlayerPosition;
        float xDifference = transform.position.x - playersPosition.x;
        float yDifference = transform.position.y - playersPosition.y;
        int targetDirection;
        if (playersPosition.y > transform.position.y && Mathf.Abs(yDifference) > Mathf.Abs(xDifference))
            targetDirection = 2;
        else if (playersPosition.y < transform.position.y && Mathf.Abs(yDifference) > Mathf.Abs(xDifference))
            targetDirection = 0;
        else if (playersPosition.x > transform.position.x && Mathf.Abs(xDifference) >= Mathf.Abs(yDifference))
            targetDirection = 3;
        else
            targetDirection = 1;

        if (rb.velocity.magnitude <= 0)
        {
            if (animator.GetInteger("Direction") != targetDirection)
            {
                animator.SetInteger("Direction", targetDirection);
                animator.speed = 1.0f;
            }
        }
        else
        {
            animator.SetInteger("Direction", targetDirection);
            animator.speed = 1.0f;
        }
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag.Equals("Player")) {
			shouldMove = false;
		}
	}

	void OnCollisionExit(Collision collision) {
		if(collision.gameObject.tag.Equals("Player")) {
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

	public void  OnLocationChanged() {
		enabled = !enabled;
	}
	
	public void OnGamePaused()
    {
        _StopEnemyMovement();
	}
	
	public void OnGameUnpaused()
    {
        _UnstopEnemyMovement();
	}

    public void _Init(float defaultHealth)
    {
        curHealth = defaultHealth;
    }

    public void _ChangeHealth(float difference)
    {
        curHealth += difference;
        if (curHealth <= 0.0f)
            Zelda._Game._EnemiesManager._KillEnemy(this);
    }

    public void _StopEnemyMovement()
    {
        savedVelocity = rb.velocity;
        rb.isKinematic = true;
        enabled = false;
        rangedEnemyComponent._StopFiring();
    }

    public void _UnstopEnemyMovement()
    {
        rb.isKinematic = false;
        rb.AddForce(savedVelocity, ForceMode.VelocityChange);
        enabled = true;
        rangedEnemyComponent._UnstopFiring();
    }

    private void AnimationFinished()
    {
        animator.speed = 0.0f;
    }
	
}
