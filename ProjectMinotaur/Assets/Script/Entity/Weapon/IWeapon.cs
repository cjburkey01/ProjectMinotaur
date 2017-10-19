public interface IWeapon {

	string GetName();

	string GetDescription();

	void OnUpdate(PlayerCombatHandler combatHandler);

	// Left click
	void OnPrimary(PlayerCombatHandler combatHandler);

	// Right click
	void OnSecondary(PlayerCombatHandler combatHandler);

	// Middle click
	void OnTertiary(PlayerCombatHandler combatHandler);
	
}