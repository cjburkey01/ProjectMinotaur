using UnityEngine;

public class Player : Entity {

	public PlayerUI PlayerOverlay { private set; get; }
	public PlayerMove MovementMotor { private set; get; }
	public Camera LookCamera { private set; get; }
    public Camera HandRenderer { private set; get; }
	public Inventory MainInventory { private set; get; }
	public Hotbar Toolbar { private set; get; }
	public UIInventory inventoryUi;
	public bool InventoryOpen { private set; get; }

	private bool didInit;

	void Start() {
        Init();
    }

	public void Init() {
		if (didInit) {
			return;
		}
		didInit = true;
		DefaultWeapons.Initialize();
		InitVars();
		InitInventory();
		PMEventSystem.GetEventSystem().AddListener<StateChangeEvent>(e => {
			if (e.Handler.State.Equals(GameState.INGAME)) {
                if (Toolbar != null) {
                    Toolbar.Exit();
                }
				InitVars();
				InitInventory();
			}
		});
	}

	public void ToggleInventory() {
		inventoryUi.gameObject.SetActive(!InventoryOpen);
		InventoryOpen = inventoryUi.gameObject.activeInHierarchy;
		if (InventoryOpen) {
			inventoryUi.RefreshInventory();
		}
	}

	void Update() {
		MovementMotor.lockCursor = GameHandler.paused || InventoryOpen;
		if (!GameHandler.paused && Toolbar != null) {
			if (Input.GetKeyDown(KeyCode.E) || (!GameHandler.paused && InventoryOpen && Input.GetButtonDown("Cancel"))) {
				ToggleInventory();
			}
			if (Input.GetKeyDown(KeyCode.Tab)) {
				Toolbar.SwitchWeapon();
			}
			if (Toolbar.GetWeapon() != null && !InventoryOpen) {
				if (Toolbar.GetWeapon().WeaponType.auto) {
					if (Input.GetButton("Fire1")) {
						Toolbar.GetWeapon().AttemptFire();
					}
				} else {
					if (Input.GetButtonDown("Fire1")) {
						Toolbar.GetWeapon().AttemptFire();
					}
				}
			}
		}
	}

	private void InitInventory() {
		MainInventory = new Inventory("PlayerInventoryMain", 15 + 2);	// 15 main inventory slots, 2 weapon slots
		Toolbar = new Hotbar(this);

		Toolbar.SetWeapon(true, Weapon.Create(this, DefaultWeapons.AutomaticRifle));
		Toolbar.SetWeapon(false, Weapon.Create(this, DefaultWeapons.Dagger));

		Debug.Log("Primary weapon: " + Toolbar.GetWeapon());

		inventoryUi.SetInventory(MainInventory);
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

		GameObject[] foundHands = GameObject.FindGameObjectsWithTag("HandRender");
		if (foundHands.Length > 0 && foundHands[0] != null) {
			HandRenderer = foundHands[0].GetComponent<Camera>();
			if (HandRenderer == null) {
				Debug.LogError("HandRenderer not found");
				Destroy(gameObject);
			}
		}
	}

}