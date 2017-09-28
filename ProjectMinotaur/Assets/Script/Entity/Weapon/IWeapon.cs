public interface IWeapon {

	string GetName();
	string GetDescription();

	// Left click
	void OnPrimary(PlayerCombatHandler combatHandler);

	// Right click
	void OnSecondary(PlayerCombatHandler combatHandler);

	// Middle click
	void OnTertiary(PlayerCombatHandler combatHandler);
	
}