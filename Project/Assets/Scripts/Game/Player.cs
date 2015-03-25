using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	//////////////////////////////////////////////////////////////////////////////////
	#region Properties

	public float maxSpeed = 2f;
	public float acceleration = 0.2f;

	private Rigidbody2D playerRigidbody = null;
	private Vector2 currentAcceleration = Vector2.zero;
	private bool isAcceleratingX = false;
	private bool isAcceleratingY = false;


	private bool locked = false;
	public bool _Locked
	{
		get { return locked; }
	}
	
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

		playerRigidbody = this.GetComponent<Rigidbody2D> ();
	}

	public void Update() 
	{
		UpdateVelocity ();
	}
	
	#endregion
	//////////////////////////////////////////////////////////////////////////////////
	#region InsideMethods
	
	private void OnInputUsed(InputManager.InputData inputData)
	{
		if (inputData.usedKeyType == InputManager.EKeyUseType.pressed) {

			CalculateVelocity (inputData, acceleration, 1);
		} else if(inputData.usedKeyType == InputManager.EKeyUseType.released) {

			if(inputData.usedKey == KeyCode.D || inputData.usedKey == KeyCode.A) {
				isAcceleratingX = false;
				//currentAcceleration.x = -Mathf.Sign(playerRigidbody.velocity.x) * deceleration;
				currentAcceleration.x = 0;
			}

			if(inputData.usedKey == KeyCode.S || inputData.usedKey == KeyCode.W) {
				isAcceleratingY = false;
				//currentAcceleration.y = -Mathf.Sign(playerRigidbody.velocity.y) * deceleration;
				currentAcceleration.y = 0;
			}
		}
	}

	private void CalculateVelocity(InputManager.InputData inputData, float acceleration, int modifier) {
		if (inputData.usedKey == KeyCode.W) {
			isAcceleratingY = modifier > 0;
			currentAcceleration.y = acceleration * modifier;
		} else if (inputData.usedKey == KeyCode.S) {
			isAcceleratingY = modifier > 0;
			currentAcceleration.y = -acceleration * modifier;
		}

		if (inputData.usedKey == KeyCode.D) {
			isAcceleratingX = modifier > 0;
			currentAcceleration.x = acceleration * modifier;
		} else if (inputData.usedKey == KeyCode.A) {
			isAcceleratingX = modifier > 0;
			currentAcceleration.x = -acceleration * modifier;
		}
	}

	private void UpdateVelocity() {
	
		Vector2 playerVelocity = playerRigidbody.velocity;
		playerVelocity += currentAcceleration;

		if (playerVelocity.magnitude > maxSpeed) {
			playerVelocity = (playerVelocity / playerVelocity.magnitude) * maxSpeed;
		}

		playerRigidbody.velocity = playerVelocity;

		//calculate deceleration
		/*
		if (!isAcceleratingX) {
			if((currentAcceleration.x < 0.0f && playerVelocity.x < 0.0f) ||
			   (currentAcceleration.x > 0.0f && playerVelocity.x > 0.0f)) {

				currentAcceleration.x = 0.0f;
				playerVelocity.x = 0.0f;
			}
		}	

		if (!isAcceleratingY) {
			if((currentAcceleration.y < 0.0f && playerVelocity.y < 0.0f) ||
			   (currentAcceleration.y > 0.0f && playerVelocity.y > 0.0f)) {

				currentAcceleration.y = 0.0f;
				playerVelocity.y = 0.0f;
			}
		}
		*/

		playerRigidbody.velocity = playerVelocity;
	}
	
	#endregion
}