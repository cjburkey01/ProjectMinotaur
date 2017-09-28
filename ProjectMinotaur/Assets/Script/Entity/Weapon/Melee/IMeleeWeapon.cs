using UnityEngine;

public class IMeleeWeapon : IWeapon {

	public float damagePerHit = 15.0f;
	public float cooldown = 1.0f;
	public float maxDistance = 2.0f;

	private float time = 0.0f;

	public virtual string GetName() {
		return "ree";
	}

	public virtual string GetDescription() {
		return "ree";
	}

	public void OnUpdate(PlayerCombatHandler combatHandler) {
		if (!GameHandler.paused) {
			time += Time.deltaTime;
		}
	}

	public void OnPrimary(PlayerCombatHandler combatHandler) {
		if (!GameHandler.paused) {
			if (time >= cooldown) {
				time -= cooldown;
				Attack(combatHandler);
			}
		}
	}

	public void OnSecondary(PlayerCombatHandler combatHandler) {
	}

	public void OnTertiary(PlayerCombatHandler combatHandler) {
	}

	private void Attack(PlayerCombatHandler ply) {
		RaycastHit hit;
		bool raycast = Physics.Raycast(ply.GetCamera().transform.position, ply.GetCamera().transform.forward, out hit, maxDistance);
		if (raycast) {
			Debug.Log("Hit: " + hit.collider.name);
		}
	}

}