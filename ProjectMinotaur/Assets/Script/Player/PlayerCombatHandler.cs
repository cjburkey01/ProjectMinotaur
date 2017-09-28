using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatHandler : IHealthHaver {

	private PlayerMove player;
	private Camera cam;

	private IWeapon weapon;

	void Start() {
		player = GetComponent<PlayerMove>();
		if (player == null) {
			Debug.LogError("Player not found on PlayerCombat object.");
			gameObject.SetActive(false);
		}

		cam = GetComponentInChildren<Camera>();
		if (cam == null) {
			Debug.LogError("Camera not found on Player object.");
			gameObject.SetActive(false);
		}

		weapon = new WeaponFist();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			weapon.OnPrimary(this);
		} else if(Input.GetMouseButtonDown(1)) {
			weapon.OnSecondary(this);
		} else if(Input.GetMouseButtonDown(2)) {
			weapon.OnTertiary(this);
		}
		weapon.OnUpdate(this);
	}

	public PlayerMove GetPlayer() {
		return player;
	}

	public Camera GetCamera() {
		return cam;
	}

}