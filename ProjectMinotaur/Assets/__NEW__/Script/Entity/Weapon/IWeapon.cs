public abstract class IWeapon {

	public abstract string GetName();

	public abstract string GetDescription();

	public abstract void OnUpdate(Player combatHandler);

	// Left click
	public abstract void OnPrimary(Player combatHandler);

	// Right click
	public abstract void OnSecondary(Player combatHandler);

	// Middle click
	public abstract void OnTertiary(Player combatHandler);

}