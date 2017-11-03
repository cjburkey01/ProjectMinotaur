using UnityEngine;

public class IHealthHaver : MonoBehaviour {

	public byte health { private set; get; }

	public void AddHealth(byte addH) {
		health += addH;
		Clamp();
	}

	public void TakeHealth(byte takeH) {
		health -= takeH;
		Clamp();
	}

	public void SetHealth(byte setH) {
		health = setH;
		Clamp();
	}

	public void Damage(object[] data) {
		byte amt = (byte) data[0];
		EntityDamageOrigin damageOrigin = (EntityDamageOrigin) data[1];
		IHealthHaver damager = (IHealthHaver) data[2];
		TakeHealth(amt);
		print("Damage: " + amt + " From: " + damager.gameObject.name);
	}

	private void Clamp() {
		health = (byte) Mathf.Clamp(health, 0, 100);
	}

}