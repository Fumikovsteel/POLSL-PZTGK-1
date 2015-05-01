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
	private Vector2 currentAcceleration = Vector2.zero;
	private bool isAcceleratingX = false;
	private bool isAcceleratingY = false;

    private Transform gameCameraTransform;
	
	private bool locked = false;
	public bool _Locked
	{
		get { return locked; }
	}

    public Vector3 _PlayerPosition
    {
        get { return transform.position; }
    }

    private const int maxLife = 100;
    private int life = maxLife;

    public event Action<int, int> _OnHealthChanged = (x, y) => { };
	
	#endregion
	//////////////////////////////////////////////////////////////////////////////////
	#region InitializationManager
	
	public void Awake()
	{
		Zelda._Game._InputManager.RegisterOnInput(new InputManager.InputKeyTaker() { _CanTakeInput = () => { return !_Locked; }, _OnInputUsed = OnInputUsed },
		new InputManager.KeyData() { keyCode = KeyCode.W, keyType = InputManager.EKeyUseType.pressedAndReleased },
		new InputManager.KeyData() { keyCode = KeyCode.S, keyType = InputManager.EKeyUseType.pressedAndReleased },
		new InputManager.KeyData() { keyCode = KeyCode.A, keyType = InputManager.EKeyUseType.pressedAndReleased },
		new InputManager.KeyData() { keyCode = KeyCode.D, keyType = InputManager.EKeyUseType.pressedAndReleased });

		playerRigidbody = this.GetComponent<Rigidbody> ();

        Zelda._Common._GameplayEvents._OnSceneWillChange += OnLevelWillChange;
        Zelda._Common._GameplayEvents._OnLocationChanged += OnLocationChanged;
	}

    public void Start()
    {
        gameCameraTransform = Zelda._Game._GameManager._GameCamera.transform;
        _OnHealthChanged(life, maxLife);
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
    }

    private void OnDestroy()
    {
        if (Zelda._Common != null)
        {
            Zelda._Common._GameplayEvents._OnSceneWillChange -= OnLevelWillChange;
            Zelda._Common._GameplayEvents._OnLocationChanged -= OnLocationChanged;
        }
    }
	
	private void OnInputUsed(InputManager.InputData inputData)
	{
		if (inputData.usedKeyType == InputManager.EKeyUseType.pressed) {

			CalculateVelocity (inputData, acceleration, 1);
		} else if(inputData.usedKeyType == InputManager.EKeyUseType.released) {

			CalculateVelocity (inputData, acceleration, -1);
		}
	}

	private void CalculateVelocity(InputManager.InputData inputData, float acceleration, int modifier) {
		if (inputData.usedKey == KeyCode.W) {

			isAcceleratingY = modifier > 0;
			currentAcceleration.y += acceleration * modifier;
		} 
		if (inputData.usedKey == KeyCode.S) {

			isAcceleratingY = modifier > 0;
			currentAcceleration.y += -acceleration * modifier;
		}

		if (inputData.usedKey == KeyCode.D) {

			isAcceleratingX = modifier > 0;
			currentAcceleration.x += acceleration * modifier;
		}
		if (inputData.usedKey == KeyCode.A) {

			isAcceleratingX = modifier > 0;
			currentAcceleration.x += -acceleration * modifier;
		}
	}

	private void UpdateVelocity() {
	
		Vector2 playerVelocity = playerRigidbody.velocity;
		playerVelocity += currentAcceleration;

		if (playerVelocity.magnitude > maxSpeed) {
			playerVelocity = (playerVelocity / playerVelocity.magnitude) * maxSpeed;
		}

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
	
	#endregion

	#region OutsideMethods

	public void TakeLife(int amount) {
		life -= amount;
		if (life <= 0) {
			Debug.Log("you are dead");
		}
        _OnHealthChanged(life, maxLife);
	}

	#endregion
}