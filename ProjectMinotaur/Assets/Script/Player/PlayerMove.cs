using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {

	public float mouseSensitivity = 1.0f;
	public float playerWalkSpeed = 1.0f;
	public float playerRunSpeed = 2.0f;
	public float playerJumpHeight = 2.0f;
	public float playerGravity = 0.2f;

	private CharacterController controller;
	private float currentSpeed;
	private Vector3 moveDirection;

	// Init vars
	void Start() {
		currentSpeed = playerWalkSpeed;
		moveDirection = Vector3.zero;
		controller = GetComponent<CharacterController>();
		if (controller == null) {
			Debug.LogError("Character controller could not be initialized. Please be sure that the player has one.");
			Destroy(gameObject);
		}
	}

	// Handle mouse looking and player movement
	void Update() {
		MouseMovement();
		HandleMovement();
	}

	// Handle mouse looking
	private void MouseMovement() {

	}

	// Handle pedal movement
	private void HandleMovement() {
		if (controller.isGrounded) {
			moveDirection.x = Input.GetAxisRaw("Horizontal");
			moveDirection.y = 0;
			moveDirection.z = Input.GetAxisRaw("Vertical");
			moveDirection.Normalize();
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= currentSpeed;
			if (Input.GetButtonDown("Jump")) {
				moveDirection.y = playerJumpHeight;
			}
		} else {
			moveDirection.y -= playerGravity * Time.deltaTime;
		}
		controller.Move(moveDirection * Time.deltaTime);
	}

}