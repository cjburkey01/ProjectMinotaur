public class WeaponFist : IMeleeWeapon {

	public WeaponFist() {
		this.damagePerHit = 10.0f;
		this.cooldown = 0.75f;
		this.maxDistance = 1.75f;
	}

	public override string GetName() {
		return "Fists";
	}

	public override string GetDescription() {
		return "Some nice, firm, strong fists to destroy them enemies (slowly).";
	}

}