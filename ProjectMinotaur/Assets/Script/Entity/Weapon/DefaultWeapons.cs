using UnityEngine;

public static class DefaultWeapons {

	public static WeaponDefinition AutomaticRifle { private set; get; }
	public static WeaponDefinition Dagger { private set; get; }
	public static WeaponDefinition Fist { private set; get; }
	public static WeaponDefinition Handgun { private set; get; }
	public static WeaponDefinition Shotgun { private set; get; }

	public static void Initialize() {
		AutomaticRifle = WeaponLoader.LoadWeapon("Weapon/Data/AutomaticRifle");
		Dagger = WeaponLoader.LoadWeapon("Weapon/Data/Dagger");
		Fist = WeaponLoader.LoadWeapon("Weapon/Data/Fist");
		Handgun = WeaponLoader.LoadWeapon("Weapon/Data/Handgun");
		Shotgun = WeaponLoader.LoadWeapon("Weapon/Data/Shotgun");

		Debug.Log("Loaded: " + AutomaticRifle);
		Debug.Log("Loaded: " + Dagger);
		Debug.Log("Loaded: " + Fist);
		Debug.Log("Loaded: " + Handgun);
		Debug.Log("Loaded: " + Shotgun);
	}

}