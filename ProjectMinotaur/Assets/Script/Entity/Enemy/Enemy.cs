using UnityEngine;

public class Enemy : IHealthHaver {

	public int damageAmt = 10;
	public float timeBetweenDamage = 1.0f;

	private float timeSinceLastDamage = 0.0f;

	public void OnCollisionStay(Collision collision) {
		if (timeSinceLastDamage >= timeBetweenDamage) {
			if () {

			}
			collision.collider.SendMessage("Damage", new object[] { (byte) damageAmt, EntityDamageOrigin.ENEMY_DAMAGE, this });
			timeSinceLastDamage = 0.0f;
		}
	}

	void Update() {
		if (timeSinceLastDamage < timeBetweenDamage * 2) {
			timeSinceLastDamage += Time.deltaTime;
		}
	}

}