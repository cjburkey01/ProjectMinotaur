public interface IWeapon {

	string GetName();

	string GetDescription();

	void OnUpdate(Player combatHandler);

	// Left click
	void OnPrimary(Player combatHandler);

	// Right click
	void OnSecondary(Player combatHandler);

	// Middle click
	void OnTertiary(Player combatHandler);

}