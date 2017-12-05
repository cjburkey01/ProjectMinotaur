using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {

	public float mouseSensitivity = 1.5f;
	public float playerMoveDamp = 0.1f;
	public float playerWalkSpeed = 6.0f;
	public float playerRunSpeed = 8.0f;
	public float playerInAirRatio = 0.8f;
	public float playerJumpHeight = 6.0f;
	public float playerGravity = 9.0f;
	public float armRotation;
	public GameObject playerArm;

	public bool Running { private set; get; }

	public bool Moving { private set; get; }

	private CharacterController controller;
	private GameObject playerCamera;
	private float currentSpeed;
	private Vector3 moveDirection;
	private Vector2 walkInput;
	private Vector2 walkMove;
	private Vector2 rotation;
	private Vector2 moveVel;
	private Vector3 previousPosition;
	private Vector3 rotationOffset;
	private float pedalSpeed;

	// Initialize variables
	void Start() {
		GameHandler.Unpause();
		currentSpeed = playerWalkSpeed;
		moveDirection = Vector3.zero;
		rotation = Vector3.zero;
		rotationOffset = new Vector3(playerArm.transform.rotation.x, playerArm.transform.rotation.y, playerArm.transform.rotation.z);

		controller = GetComponent<CharacterController>();
		if (controller == null) {
			Debug.LogError("Character controller could not be initialized. Please be sure that the player has one.");
			gameObject.SetActive(false);
		}

		Camera cam = GetComponentInChildren<Camera>();
		if (cam == null) {
			Debug.LogError("Camera could not be found. Please be sure that the player has one.");
			gameObject.SetActive(false);
		}
		playerCamera = cam.gameObject;
	}

	// Handle movement, in general; called every single frame
	void Update() {
		if (!GameHandler.paused) {
			MouseMovement();
			HandleMovement();
		}
	}

	// Handle mouse-look
	private void MouseMovement() {
		rotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
		rotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		rotation.y = Mathf.Clamp(rotation.y, -90.0f, 90.0f);

		transform.localRotation = Quaternion.Euler(new Vector3(0, rotation.x, 0));
		playerCamera.transform.localRotation = Quaternion.Euler(new Vector3(rotation.y, 0, 0));
	}

	// Handle pedal(foot-based) movement
	private void HandleMovement() {
		HandleWalking();
		HandleJumping();
		HandleGravity();
		controller.Move(moveDirection * Time.deltaTime);
		Moving = previousPosition != transform.position;
		previousPosition = transform.position;
	}

	// Handle walking, specifically
	private void HandleWalking() {
		Running = Input.GetKey(KeyCode.LeftShift);
		currentSpeed = ((Running) ? (playerRunSpeed) : (playerWalkSpeed));
		walkInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		walkMove = Vector2.SmoothDamp(walkMove, walkInput.normalized, ref moveVel, playerMoveDamp, 100.0f, Time.deltaTime);
		pedalSpeed = ((controller.isGrounded) ? (currentSpeed) : (currentSpeed * playerInAirRatio));
		moveDirection.x = walkMove.x * pedalSpeed;
		moveDirection.z = walkMove.y * pedalSpeed;
		moveDirection = transform.TransformDirection(moveDirection);
	}

	// Handle jumping, specifically
	private void HandleJumping() {
		if (controller.isGrounded && Input.GetButton("Jump")) {
			moveDirection.y = playerJumpHeight;
		}
	}

	// Handle gravity, specifically
	private void HandleGravity() {
		if (!controller.isGrounded) {
			moveDirection.y -= playerGravity * Time.deltaTime;
		}
	}

	//controls arm rotation
	private void HandleArmRotation() {
		while (Moving) {
			if (Mathf.Approximately(playerArm.transform.rotation.x, armRotation)) {
				rotationOffset = rotationOffset + new Vector3(armRotation, 0, 0);
			}
		}
	}

	public bool Grounded() {
		return controller.isGrounded;
	}

}