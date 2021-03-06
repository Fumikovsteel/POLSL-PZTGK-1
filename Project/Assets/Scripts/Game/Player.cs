using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	//////////////////////////////////////////////////////////////////////////////////
	#region Properties

	public float maxSpeed = 2.0f;
	public float acceleration = 0.2f;
	public float decelerationModifier = 2.0f;

    private Rigidbody playerRigidbody = null;
	private Vector3 currentAcceleration = Vector3.zero;

    private Transform gameCameraTransform;
	private Animator animator;

    public bool _Locked;

	private bool isInputXDirty = false;
	private bool isInputYDirty = false;

	// how long the player has to go in given direction before he actually rotates
	// set 0 for instant rotation
	public int rotationDelay = 5;
	private int rotationReadyCounter = 0;

    [SerializeField]
    private SpriteRenderer swordObject;
    [SerializeField]
    private EquipmentManager.Stock[] startEquipmentState;
    [SerializeField]
    private float deathAnimationTime;
    [SerializeField]
    private Vector3 deathAnimationAmount;
    /// <summary>
    /// Target player rotation (depends on movement direction) is applied to this game object
    /// </summary>
    [SerializeField]
    private Transform playersRotationParent;
    [SerializeField]
    private GameObject pressSpaceToAttackPrefab;

    public Vector3 _PlayerPosition
    {
        get { return transform.position; }
    }

	public Vector3 _PlayerDirection
	{
		get { return playerRigidbody.velocity.normalized; }
	}

    public List<EquipmentManager.Stock> _AllEquipmentItems
    { get { return equipmentManager._GetEquipmentState(); } }

    /// <summary>
    /// When player collect some item from map
    /// </summary>
    public event Action<EquipmentItems.EquipmentItem> _OnItemGathered;

    /// <summary>
    /// When player uses a mixture
    /// </summary>
    public event Action<EquipmentItems.EquipmentItem> _OnMixtureUsed;

    private const int maxLife = 100;
    private int life = maxLife;

    public event Action<int, int> _OnHealthChanged = (x, y) => { };

    private EquipmentManager equipmentManager;
	private PlayerMeleeAttack meleeAttack;
	
	#endregion
	//////////////////////////////////////////////////////////////////////////////////
	#region InitializationManager
	
	public void Awake()
	{
        RegisterInput();

		playerRigidbody = this.GetComponent<Rigidbody> ();
        animator = this.GetComponent<Animator>();

        Zelda._Common._GameplayEvents._OnSceneWillChange += OnLevelWillChange;
        Zelda._Common._GameplayEvents._OnLocationChanged += OnLocationChanged;
        Zelda._Common._GameplayEvents._OnGamePaused += OnGamePaused;
        Zelda._Common._GameplayEvents._OnGameUnpaused += OnGameUnpaused;

        meleeAttack = GetComponent<PlayerMeleeAttack>();
        meleeAttack._OnEnemyHit += OnEnemyHit;
	}

    public void Start()
    {
        gameCameraTransform = Zelda._Game._GameManager._GameCamera.transform;
        _OnHealthChanged(life, maxLife);

        equipmentManager = new EquipmentManager(transform);
        equipmentManager._OnItemGathered += (x) => _OnItemGathered(x);
        equipmentManager._OnMixtureUsed += (x) => _OnMixtureUsed(x);

        _OnItemGathered += OnItemGathered;

        for (int i = startEquipmentState.Length - 1; i >= 0; i--)
            equipmentManager._AddToEquipment(startEquipmentState[i]._EquipmentItem, startEquipmentState[i]._Count);
    }

	public void Update() 
	{
		UpdateVelocity ();
        gameCameraTransform.position = new Vector3(transform.position.x, transform.position.y, gameCameraTransform.position.z);
	}
	
	#endregion
	//////////////////////////////////////////////////////////////////////////////////
	#region InsideMethods

    private void RegisterInput()
    {
        Zelda._Game._InputManager.RegisterOnInput(new InputManager.InputKeyTaker()
            {
                _CanTakeInput = () => { return !_Locked; },
                _OnInputUsed = OnInputUsed
            }, 
            new InputManager.KeyData() { keyCode = KeyCode.W, keyType = InputManager.EKeyUseType.pressedAndReleased },
            new InputManager.KeyData() { keyCode = KeyCode.S, keyType = InputManager.EKeyUseType.pressedAndReleased },
            new InputManager.KeyData() { keyCode = KeyCode.A, keyType = InputManager.EKeyUseType.pressedAndReleased },
            new InputManager.KeyData() { keyCode = KeyCode.D, keyType = InputManager.EKeyUseType.pressedAndReleased });

        Zelda._Game._InputManager.RegisterOnInput(new InputManager.InputKeyTaker()
            {
                _CanTakeInput = () => { return !_Locked; },
                _OnInputUsed = (x) => equipmentManager._UseWeapon(this)
            }, new InputManager.KeyData()
            {
                keyCode = KeyCode.Space, keyType = InputManager.EKeyUseType.released
            });

        Zelda._Game._InputManager.RegisterOnInput(new InputManager.InputKeyTaker()
            {
                _CanTakeInput = () => { return !_Locked; },
                _OnInputUsed = (x) => equipmentManager._UseMixture(this, EquipmentManager.EEquipmentItem.SpeedMixture),
            }, new InputManager.KeyData()
            {
                keyCode = KeyCode.Alpha2, keyType = InputManager.EKeyUseType.released
            });

        Zelda._Game._InputManager.RegisterOnInput(new InputManager.InputKeyTaker()
            {
                _CanTakeInput = () => { return !_Locked; },
                _OnInputUsed = (x) => equipmentManager._UseMixture(this, EquipmentManager.EEquipmentItem.HealthMixture)
            }, new InputManager.KeyData()
            {
                keyCode = KeyCode.Alpha1, keyType = InputManager.EKeyUseType.released
            });
    }

    private void OnLevelWillChange(SceneManager.ESceneName newScene)
    {
        // Because it has DontDestroyOnLoad
        if (newScene != SceneManager.ESceneName.Game)
            Destroy(gameObject);
    }

    private void OnLocationChanged()
    {
        transform.position = PlayerSpawnPosition._GetSpawnPosition(Zelda._Game._LocationInitData._TargetSpawnPosition, Zelda._Game._LocationInitData._TargetLocationName).transform.position;
		playerRigidbody.velocity = Vector3.zero;

		// dirty flags
		if (currentAcceleration.x != 0) {
			isInputXDirty = true;
		}
		if (currentAcceleration.y != 0) {
			isInputYDirty = true;
		}
		currentAcceleration = Vector3.zero;
    }

    private void OnGamePaused()
    {
        _Locked = true;
    }

    private void OnGameUnpaused()
    {
        _Locked = false;
    }

    private void OnDestroy()
    {
        if (Zelda._Common != null)
        {
            Zelda._Common._GameplayEvents._OnSceneWillChange -= OnLevelWillChange;
            Zelda._Common._GameplayEvents._OnLocationChanged -= OnLocationChanged;
            Zelda._Common._GameplayEvents._OnGamePaused -= OnGamePaused;
            Zelda._Common._GameplayEvents._OnGameUnpaused -= OnGameUnpaused;
        }
    }

    private void KillPlayerAnimationFinished()
    {
        Destroy(gameObject);
        Zelda._Common._GameplayEvents.RaiseOnPlayerKilled();
    }

    private void KillPlayer()
    {
        // We don't want camera to shake with player
        enabled = false;
        OnGamePaused();
        iTween.ShakePosition(gameObject, iTween.Hash("amount", deathAnimationAmount, "time", deathAnimationTime, "islocal", true,
                                                     "oncomplete", "KillPlayerAnimationFinished"));
        iTween.ShakePosition(gameCameraTransform.gameObject, iTween.Hash("amount", deathAnimationAmount / 3.0f, "time", deathAnimationTime, "islocal", true));
        Zelda._Common._GameplayEvents.RaiseOnPlayerWillBeKilled();
    }

    private void OnEnemyHit(Enemy hittedEnemy, float attackStrength, float attackRecoild)
    {
        Rigidbody enemyBody = hittedEnemy.GetComponent<Rigidbody>();
        if (enemyBody != null)
        {
            Vector3 forceDirection = new Vector3(hittedEnemy.transform.position.x - transform.position.x, hittedEnemy.transform.position.y - transform.position.y, 0.0f);
            enemyBody.AddForce(forceDirection.normalized * attackRecoild, ForceMode.Impulse);
        }
        hittedEnemy._ChangeHealth(-attackStrength);
    }

    private void OnItemGathered(EquipmentItems.EquipmentItem newItem)
    {
        if (newItem._ItemType == EquipmentManager.EEquipmentType.weapon)
        {
            // If we have sword first time
            if (swordObject.sprite == null)
            {
                GameObject pressSpaceInstantiatedObject = Instantiate(pressSpaceToAttackPrefab) as GameObject;
                pressSpaceInstantiatedObject.transform.SetParentResetLocal(transform);
                pressSpaceInstantiatedObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f,
                                                                              pressSpaceToAttackPrefab.transform.position.z);
                Action<EquipmentItems.EquipmentItem> onWeaponUsed = null;
                onWeaponUsed = (x) =>
                    {
                        equipmentManager._OnWeaponUsed -= onWeaponUsed;
                        Destroy(pressSpaceInstantiatedObject);
                    };
                equipmentManager._OnWeaponUsed += onWeaponUsed;
            }
            swordObject.sprite = newItem._ItemSprite;
        }
    }

    private IEnumerator RevertSpeedMixture(float waitTime, float defaultMaxSpeed, float defaultAcceleration)
    {
        yield return new WaitForSeconds(waitTime);

        maxSpeed = defaultMaxSpeed;
        //acceleration = defaultAcceleration;
    }
	
	private void OnInputUsed(InputManager.InputData inputData)
	{
		if (inputData.usedKeyType == InputManager.EKeyUseType.pressed) {

			CalculateVelocity (inputData, acceleration, 1);
	
		}
		else if(inputData.usedKeyType == InputManager.EKeyUseType.released) {

			CalculateVelocity (inputData, acceleration, -1);
		}
	}

	private void CalculateVelocity(InputManager.InputData inputData, float acceleration, int modifier) {
		
		if (inputData.usedKey == KeyCode.W) {
			currentAcceleration.y += acceleration * modifier;
			rotationReadyCounter = -rotationDelay;

			if(isInputYDirty && modifier < 0) {
				currentAcceleration.y = 0.0f;
				isInputYDirty = false;
			} else {
				isInputYDirty = false;
			}
		} 

		if (inputData.usedKey == KeyCode.S) {
			currentAcceleration.y += -acceleration * modifier;
			rotationReadyCounter = -rotationDelay;

			if(isInputYDirty && modifier < 0) {
				currentAcceleration.y = 0.0f;
				isInputYDirty = false;
			} else {
				isInputYDirty = false;
			}
		}
			
		if (inputData.usedKey == KeyCode.D) {
			currentAcceleration.x += acceleration * modifier;
			rotationReadyCounter = -rotationDelay;

			if(isInputXDirty && modifier < 0) {
				currentAcceleration.x = 0.0f;
				isInputXDirty = false;
			} else {
				isInputXDirty = false;
			}
		}
		if (inputData.usedKey == KeyCode.A) {
			currentAcceleration.x += -acceleration * modifier;
			rotationReadyCounter = -rotationDelay;

			if(isInputXDirty && modifier < 0) {
				currentAcceleration.x = 0.0f;
				isInputXDirty = false;
			} else {
				isInputXDirty = false;
			}
		}
	}

	private void UpdateVelocity() {

		if(rotationReadyCounter < 0) {
			++rotationReadyCounter;
		}
	
		Vector3 playerVelocity = playerRigidbody.velocity;
		playerVelocity += currentAcceleration;

		if (playerVelocity.magnitude > maxSpeed) {
			playerVelocity = (playerVelocity / playerVelocity.magnitude) * maxSpeed;
		}

		if (currentAcceleration.magnitude > 0.0f) {
			playerRigidbody.drag = 0.0f;
		} else {
			playerRigidbody.drag = maxSpeed * decelerationModifier;
		}

		// we want to rotate only on user input
		if (currentAcceleration.magnitude > 0.0f && rotationReadyCounter >= 0) {
			
			float rotationX = 0.0f;
			float rotationY = 0.0f;
			
			if (currentAcceleration.x > 0) {
				rotationX = 90.0f;
			} else if (currentAcceleration.x < 0) {
				rotationX = -90.0f;
			}
			
			if (currentAcceleration.y > 0) {
				rotationY = Mathf.Sign (rotationX) * 180.0f;
			} else if (currentAcceleration.y < 0) {
				rotationY = 0.0f;
			}
			
			float divider = ( (currentAcceleration.x != 0.0f) && (currentAcceleration.y != 0.0f)) ? 2.0f : 1.0f;
			float rotationZ = (rotationX + rotationY) / divider;
            // rotationZ need to be higher than 0
            SetProperlyRotation((rotationZ + 360.0f) % 360.0f);
		}

        animator.speed = currentAcceleration.magnitude > 0 ? 1 : 0;
		playerRigidbody.velocity = playerVelocity;
	}

    private void SetProperlyRotation(float targetRotationZ)
    {
        if (targetRotationZ >= 314 || targetRotationZ <= 46)
        {
            animator.SetInteger("Direction", 0);
            playersRotationParent.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else if (targetRotationZ >= 134 && targetRotationZ <= 226)
        {
            animator.SetInteger("Direction", 2);
            playersRotationParent.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
        }
        else if (targetRotationZ >= 89 && targetRotationZ <= 91)
        {
            animator.SetInteger("Direction", 3);
            playersRotationParent.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        }
        else
        {
            animator.SetInteger("Direction", 1);
            playersRotationParent.rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
        }
    }

	#endregion

	#region OutsideMethods

	public void TakeLife(int amount) {
        // Shield can sometimes bounce projectiles
        if (UnityEngine.Random.Range(0.0f, 100.0f) > equipmentManager._ShieldValue)
        {
            // Armor decrease damage
            amount = (int)((float)amount * (1.0f - (1.0f * equipmentManager._ArmorValue / 100.0f)));
            life -= amount;
            if (life <= 0)
                // We delay because projectile physics need to change player position
                Zelda._Common._TimeManager._DelayMethodExecution(0.05f, KillPlayer);
            _OnHealthChanged(life, maxLife);
        }
	}

    public void UseHealthMixture(int extraHealth)
    {
        int prevLife = life;
        life = Mathf.Clamp(life + extraHealth, 0, maxLife);
        if (prevLife != life)
            _OnHealthChanged(life, maxLife);
		Zelda._Common._SoundManager.PlaySound(SoundManager.SoundName.PotionUse);
    }

    public void UseSpeedMixture(float extraSpeedValue, float boostTime)
    {
        StartCoroutine(RevertSpeedMixture(boostTime, maxSpeed, acceleration));
        maxSpeed *= extraSpeedValue;
        //acceleration *= extraSpeedValue;
		Zelda._Common._SoundManager.PlaySound(SoundManager.SoundName.PotionUse);
    }

    public void StartMeleeAttackAnimation(float attackStrength, float recoil)
    {
        meleeAttack._StartAttackAnimation(swordObject.transform, attackStrength, recoil);
    }

    public void CollectSomeItem(ICollectableObject collectableObject)
    {
        EquipmentItems.EquipmentItem equipmentItem = collectableObject as EquipmentItems.EquipmentItem;
        if (equipmentItem != null)
        {
            if (equipmentManager._AddToEquipment(equipmentItem))
                collectableObject._Collect();
        }
    }

    public float RotateToNPC(Transform npc)
    {
        float targetRotation;

        float xDifference = transform.position.x - npc.position.x;
        float yDifference = transform.position.y - npc.position.y;
        if (npc.position.y > transform.position.y && Mathf.Abs(yDifference) > Mathf.Abs(xDifference))
            targetRotation = 180.0f;
        else if (npc.position.y < transform.position.y && Mathf.Abs(yDifference) > Mathf.Abs(xDifference))
            targetRotation = 0.0f;
        else if (npc.position.x > transform.position.x && Mathf.Abs(xDifference) >= Mathf.Abs(yDifference))
            targetRotation = 90.0f;
        else
            targetRotation = 270.0f;

        SetProperlyRotation(targetRotation);
        return targetRotation;
    }

	#endregion
}