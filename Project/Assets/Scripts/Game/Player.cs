using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
	//////////////////////////////////////////////////////////////////////////////////
	#region Properties

	public float maxSpeed = 2f;
	public float acceleration = 0.2f;

    private Rigidbody playerRigidbody = null;
	private Vector3 currentAcceleration = Vector3.zero;

    private Transform gameCameraTransform;

    public bool _Locked;

	public bool isInputXDirty = false;
	public bool isInputYDirty = false;

    [SerializeField]
    private GameObject swordObject;

    public Vector3 _PlayerPosition
    {
        get { return transform.position; }
    }

	public Vector3 _PlayerDirection
	{
		get { return playerRigidbody.velocity.normalized;}
	}

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
		Zelda._Game._InputManager.RegisterOnInput(new InputManager.InputKeyTaker() { _CanTakeInput = () => { return !_Locked; }, _OnInputUsed = OnInputUsed },
		new InputManager.KeyData() { keyCode = KeyCode.W, keyType = InputManager.EKeyUseType.pressedAndReleased },
		new InputManager.KeyData() { keyCode = KeyCode.S, keyType = InputManager.EKeyUseType.pressedAndReleased },
		new InputManager.KeyData() { keyCode = KeyCode.A, keyType = InputManager.EKeyUseType.pressedAndReleased },
		new InputManager.KeyData() { keyCode = KeyCode.D, keyType = InputManager.EKeyUseType.pressedAndReleased },
		new InputManager.KeyData() { keyCode = KeyCode.Space, keyType = InputManager.EKeyUseType.pressedAndReleased });

		playerRigidbody = this.GetComponent<Rigidbody> ();

        Zelda._Common._GameplayEvents._OnSceneWillChange += OnLevelWillChange;
        Zelda._Common._GameplayEvents._OnLocationChanged += OnLocationChanged;
        Zelda._Common._GameplayEvents._OnGamePaused += OnGamePaused;
        Zelda._Common._GameplayEvents._OnGameUnpaused += OnGameUnpaused;

        equipmentManager = new EquipmentManager(transform);
	}

    public void Start()
    {
        gameCameraTransform = Zelda._Game._GameManager._GameCamera.transform;
        _OnHealthChanged(life, maxLife);
		meleeAttack = GetComponent<PlayerMeleeAttack> ();
    }

	public void Update() 
	{
		UpdateVelocity ();
        gameCameraTransform.position = new Vector3(transform.position.x, transform.position.y, gameCameraTransform.position.z);
	}
	
	#endregion
	//////////////////////////////////////////////////////////////////////////////////
	#region InsideMethods

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
	
	private void OnInputUsed(InputManager.InputData inputData)
	{
		if (inputData.usedKeyType == InputManager.EKeyUseType.pressed) {

			CalculateVelocity (inputData, acceleration, 1);
			if (inputData.usedKey == KeyCode.Space) {
				Attack();
				Debug.Log("space");
			}
	
		}
		else if(inputData.usedKeyType == InputManager.EKeyUseType.released) {

			CalculateVelocity (inputData, acceleration, -1);
		}
	}

	private void CalculateVelocity(InputManager.InputData inputData, float acceleration, int modifier) {
		
		if (inputData.usedKey == KeyCode.W) {

			currentAcceleration.y += acceleration * modifier;

			if(isInputYDirty && modifier < 0) {
				currentAcceleration.y = 0.0f;
				isInputYDirty = false;
			} else {
				isInputYDirty = false;
			}
		} 

		if (inputData.usedKey == KeyCode.S) {

			currentAcceleration.y += -acceleration * modifier;

			if(isInputYDirty && modifier < 0) {
				currentAcceleration.y = 0.0f;
				isInputYDirty = false;
			} else {
				isInputYDirty = false;
			}
		}
			
		if (inputData.usedKey == KeyCode.D) {

			currentAcceleration.x += acceleration * modifier;

			if(isInputXDirty && modifier < 0) {
				currentAcceleration.x = 0.0f;
				isInputXDirty = false;
			} else {
				isInputXDirty = false;
			}
		}
		if (inputData.usedKey == KeyCode.A) {

			currentAcceleration.x += -acceleration * modifier;

			if(isInputXDirty && modifier < 0) {
				currentAcceleration.x = 0.0f;
				isInputXDirty = false;
			} else {
				isInputXDirty = false;
			}
		}
		Vector3 ads = playerRigidbody.velocity.normalized;
		Debug.Log (ads);
	}

	private void UpdateVelocity() {
	
		Vector3 playerVelocity = playerRigidbody.velocity;
		playerVelocity += currentAcceleration;

		if (playerVelocity.magnitude > maxSpeed) {
			playerVelocity = (playerVelocity / playerVelocity.magnitude) * maxSpeed;
		}

		// we want to rotate only on user input
		if (currentAcceleration.magnitude > 0.0f) {

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

			float divider = (currentAcceleration.x != 0.0f && currentAcceleration.y != 0.0f) ? 2.0f : 1.0f;
			float rotationZ = (rotationX + rotationY) / divider;

			transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationZ);

			playerRigidbody.velocity = playerVelocity;
		}
	}

	private void Attack() {
		meleeAttack.Attack ();
	}
	
	#endregion

	#region OutsideMethods

	public void TakeLife(int amount) {
		life -= amount;
		if (life <= 0) {
			Debug.Log("you are dead");
			//TODO DEATH HANDLING
		}
        _OnHealthChanged(life, maxLife);
	}

    public void UseHealthMixture(int extraHealth)
    {
        int prevLife = life;
        life = Mathf.Clamp(life + extraHealth, 0, maxLife);
        if (prevLife != life)
            _OnHealthChanged(life, maxLife);
    }

    public void UseSpeedMixture(float extraSpeedValue, float boostTime)
    {
        // TODO!!!
    }

    public void StartMeleeAttackAnimation(float attackStrength)
    {
        // TODO!!!
    }

    public void CollectSomeItem(ICollectableObject collectableObject)
    {
        EquipmentItem equipmentItem = collectableObject as EquipmentItem;
        if (equipmentItem != null)
        {
            if (equipmentManager._AddToEquipment(equipmentItem))
                collectableObject._Collect();
        }
    }

	#endregion
}