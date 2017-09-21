using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {
	
	public float playerWalkSpeed = 1.0f;
	public float playerRunSpeed = 2.0f;
	public float playerJumpHeight = 2.0f;

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

	// Handle mouse looking
	void Update() {

	}

	// Handle movement
	void FixedUpdate() {

	}

}