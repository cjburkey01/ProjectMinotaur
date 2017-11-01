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

	public void Damage(byte amt) {
		TakeHealth(amt);
	}

	private void Clamp() {
		health = Mathf.Clamp(health, 0, 100);
	}

}