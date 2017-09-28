using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHealthHaver : MonoBehaviour {

	public float health { private set; get; }

	public void AddHealth(float addH) {
		health += addH;
		Clamp();
	}

	public void TakeHealth(float takeH) {
		health -= takeH;
		Clamp();
	}

	public void SetHealth(float setH) {
		health = setH;
		Clamp();
	}

	private void Clamp() {
		health = Mathf.Clamp(health, 0.0f, 100.0f);
	}

}