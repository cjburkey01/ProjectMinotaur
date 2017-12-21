using UnityEngine;

public abstract class IMeleeWeapon : IWeapon {

	public float damagePerHit = 15.0f;
	public float cooldown = 1.0f;
	public float maxDistance = 2.0f;

	private float sinceLastAttack = 0.0f;

	public override void OnUpdate(Player combatHandler) {
		if (!GameHandler.Paused && sinceLastAttack <= cooldown) {
			sinceLastAttack += Time.deltaTime;
		}
	}

	public override void OnPrimary(Player combatHandler) {
		if (!GameHandler.Paused) {
			if (sinceLastAttack >= cooldown) {
				sinceLastAttack = 0.0f;
				Attack(combatHandler);
			}
		}
	}

	public override void OnSecondary(Player combatHandler) {
	}

	public override void OnTertiary(Player combatHandler) {
	}

	protected void Attack(Player ply) {
		RaycastHit hit;
		bool raycast = Physics.Raycast(ply.GetCamera().transform.position, ply.GetCamera().transform.forward, out hit, maxDistance);
		if (raycast) {
			Debug.Log("Hit: " + hit.collider.name);
		}
	}

}