using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMeleeWeapon : MonoBehaviour, IWeapon {

	public float damagePerHit = 15.0f;
	public float cooldown = 1.0f;

	private float time = 0.0f;

	public string GetName() {
		return "";
	}

	public string GetDescription() {
		return "";
	}

	void Update() {
		time += Time.deltaTime;
	}

	public void OnPrimary(PlayerCombatHandler combatHandler) {
		if (time >= cooldown) {
			time -= cooldown;
			Attack(combatHandler);
		}
	}

	public void OnSecondary(PlayerCombatHandler combatHandler) {
	}

	public void OnTertiary(PlayerCombatHandler combatHandler) {
	}

	private void Attack(PlayerCombatHandler ply) {
		RaycastHit hit;
		bool raycastSuccess = Physics.Raycast();
	}

}