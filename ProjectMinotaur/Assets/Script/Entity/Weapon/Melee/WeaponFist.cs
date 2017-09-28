using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFist : IMeleeWeapon {

	public string GetName() {
		return "Fists";
	}

	public string GetDescription() {
		return "Some nice, firm, strong fists to destroy them enemies.";
	}

}