using UnityEngine;

public class Player : Entity {
	
	public PlayerUI PlayerOverlay { private set; get; }
	public PlayerMove MovementMotor { private set; get; }
	public Camera LookCamera { private set; get; }
	public Inventory MainInventory { private set; get; }
	public Hotbar Toolbar { private set; get; }

	public void Start() {
		InitInventory();
		InitVars();
	}

	void Update() {
		if (PlayerOverlay != null) {
			PlayerOverlay.healthBar.progress = GetHealth() / 100.0f;
		}
	}

	private void InitInventory() {
		DefaultWeapons.Initialize();

		MainInventory = new Inventory("PlayerInventoryMain", 25);
		Toolbar = new Hotbar(this);

		Toolbar.SetWeapon(true, null);	// Set both primary and secondary weapons to fists by default.
		Toolbar.SetWeapon(false, null);

		Debug.Log("Primary weapon: " + Toolbar.GetWeapon());
	}

	private void InitVars() {
		PlayerOverlay = FindObjectOfType<PlayerUI>();
		if (PlayerOverlay == null) {
			Debug.LogWarning("Player overlay UI not found in scene.");
		}

		MovementMotor = GetComponent<PlayerMove>();
		if (MovementMotor == null) {
			Debug.LogError("Player not found on PlayerCombat object.");
			Destroy(gameObject);
		}

		LookCamera = GetComponentInChildren<Camera>();
		if (LookCamera == null) {
			Debug.LogError("Camera not found on Player object.");
			Destroy(gameObject);
		}
	}

}