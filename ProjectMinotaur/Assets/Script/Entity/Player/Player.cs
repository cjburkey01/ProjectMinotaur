using UnityEngine;

public class Player : Entity {

	public GameObject plyInventory;
	public ItemList inventoryItems;
	public ItemListWeapons inventoryWeapons;

	public PlayerUI PlayerOverlay { private set; get; }
	public PlayerMove MovementMotor { private set; get; }
	public Camera LookCamera { private set; get; }
    public Camera HandRenderer { private set; get; }
	public Inventory MainInventory { private set; get; }
	public Hotbar Toolbar { private set; get; }
	public Inventory InventoryOpen { private set; get; }

	private bool didInit;

	void Start() {
        Init();
    }

	public void Init() {
		if (didInit) {
			return;
		}
		didInit = true;
		InitVars();
		InitInventory();
		PMEventSystem.GetEventSystem().AddListener<StateChangeEvent>(DoStateInit);
		PMEventSystem.GetEventSystem().AddListener<WorldSaveEvent>(OnWorldSave);
		PMEventSystem.GetEventSystem().AddListener<WorldLoadEvent>(OnWorldLoad);
		InventoryOpen = null;
	}

	private void DoStateInit(StateChangeEvent e) {
		if (e.Handler.State.Equals(GameState.INGAME)) {
			if (Toolbar != null) {
				Toolbar.Exit();
			}
			InitVars();
			InitInventory();
		}
	}

	private void OnWorldSave(WorldSaveEvent e) {
		e.DataHandler.Set("PlayerPosition", transform.position);
		e.DataHandler.Set("PlayerRotationOnX", MovementMotor.rotation.x);
		e.DataHandler.Set("PlayerRotationOnY", MovementMotor.rotation.y);
	}

	private void OnWorldLoad(WorldLoadEvent e) {

	}

	public void TogglePlayerInventory() {
		if (InventoryOpen != null) {
			InventoryOpen = null;
			inventoryItems.CloseInventory();
			inventoryWeapons.OpenInventory();
			plyInventory.SetActive(false);
		} else {
			OpenPlayerInventory();
		}
	}

	private void OpenPlayerInventory() {
		plyInventory.SetActive(true);
		InventoryOpen = MainInventory;
		inventoryWeapons.hotbar = Toolbar;
		inventoryItems.inventory = MainInventory;
		inventoryWeapons.OpenInventory();
		inventoryItems.OpenInventory();
	}

	void Update() {
		MovementMotor.lockCursor = GameHandler.paused || InventoryOpen != null;
		if (!GameHandler.paused && Toolbar != null) {
			if (Input.GetKeyDown(KeyCode.E) || (!GameHandler.paused && InventoryOpen != null && Input.GetButtonDown("Cancel"))) {
				TogglePlayerInventory();
			}
			if (Input.GetKeyDown(KeyCode.Tab)) {
				Toolbar.SwitchWeapon();
			}
			if (Toolbar.GetWeapon() != null && InventoryOpen == null) {
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
		MainInventory = new Inventory("PlayerInventoryMain", 15);   // 15 main inventory slots
		Toolbar = new Hotbar(this, inventoryWeapons);

		//Toolbar.SetWeapon(true, Weapon.Create(this, DefaultWeapons.AutomaticRifle));
		//Toolbar.SetWeapon(false, Weapon.Create(this, DefaultWeapons.Dagger));

		MainInventory.Add(new ItemStack(DefaultWeapons.Handgun, 1));
		MainInventory.Add(new ItemStack(DefaultWeapons.Dagger, 1));
		MainInventory.Add(new ItemStack(DefaultWeapons.AutomaticRifle, 1));
		MainInventory.Add(new ItemStack(DefaultWeapons.Shotgun, 1));
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