using UnityEngine;

public class Player : Entity {

	public PlayerUI PlayerOverlay { private set; get; }
	public PlayerMove MovementMotor { private set; get; }
	public Camera LookCamera { private set; get; }
	public Inventory MainInventory { private set; get; }
	public Hotbar Toolbar { private set; get; }

	void Start() {
		InitVars();
		InitInventory();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			Toolbar.SwitchWeapon();
		}
		if (Input.GetButtonDown("Fire1")) {
			Toolbar.GetWeapon().WeaponType.OnPrimary(Toolbar.GetWeapon());
		}
	}

	private void InitInventory() {
		DefaultWeapons.Initialize();

		MainInventory = new Inventory("PlayerInventoryMain", 25);
		Toolbar = new Hotbar(this);

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