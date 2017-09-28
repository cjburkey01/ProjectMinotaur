using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatHandler : IHealthHaver {

	private PlayerMove player;

	void Start() {
		player = GetComponent<PlayerMove>();
		if (player == null) {
			Debug.LogError("Player not found on PlayerCombat object.");
			gameObject.SetActive(false);
		}
	}

	public PlayerMove GetPlayer() {
		return player;
	}

	public Vector3 GetPosition() {
		return transform.position;
	}

	public Vector3 GetCameraPosition() {
		
	}

	public Quaternion GetRotation() {
		return transform.rotation;
	}

}