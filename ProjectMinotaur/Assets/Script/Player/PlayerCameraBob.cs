﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class PlayerCameraBob : MonoBehaviour {

	public float walkingBob = 360.0f;
	public float runningBob = 480.0f;
	public float bobAmount = 0.17f;

	private Transform cameraTransform;
	private PlayerMove playerMove;
	private Vector3 originalCameraPosition;
	private Vector3 currentPosition;
	private float timer = 0.0f;
	private float bobbing = 1.0f;
	private float coef = 0.0f;
	private float smoothVel = 0.0f;

	// Initialize variables
	void Start() {
		Camera cam = GetComponentInChildren<Camera>();
		if (cam == null) {
			Debug.LogError("Camera not found on object.");
			gameObject.SetActive(false);
		}

		playerMove = GetComponent<PlayerMove>();
		if (playerMove == null) {
			Debug.LogError("Player movement script not found on object.");
			gameObject.SetActive(false);
		}

		cameraTransform = cam.gameObject.transform;
		originalCameraPosition = cameraTransform.localPosition;
		currentPosition = Vector3.zero;
	}

	// Do camera bobbing
	void Update() {
		if (playerMove.moving) {
			bobbing = ((playerMove.running) ? (runningBob) : (walkingBob));
			timer += Time.deltaTime;
		} else {
			timer = Mathf.SmoothDamp(timer, 0.0f, ref smoothVel, 0.25f);
		}
		coef = timer * bobbing;
		if (coef >= 360.0f) {
			timer %= 360.0f / bobbing;
		}
		currentPosition.y = originalCameraPosition.y + ((Mathf.Sin(Mathf.Deg2Rad * coef) / 2) * bobAmount);
		cameraTransform.localPosition = currentPosition;
	}

}