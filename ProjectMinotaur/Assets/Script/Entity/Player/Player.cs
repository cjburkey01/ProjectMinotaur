using UnityEngine;

public class Player : Entity {

	public PlayerUI PlayerOverlay { private set; get; }
	public PlayerMove MovementMotor { private set; get; }
	public Camera LookCamera { private set; get; }
    public Camera HandRenderer { private set; get; }
	public Inventory MainInventory { private set; get; }
	public Hotbar Toolbar { private set; get; }

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

	void Update() {
		if (!GameHandler.paused && Toolbar != null) {
			if (Input.GetKeyDown(KeyCode.Tab)) {
				Toolbar.SwitchWeapon();
			}
			if (Toolbar.GetWeapon() != null) {
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

        HandRenderer = GameObject.FindGameObjectsWithTag("HandRender")[0].GetComponent<Camera>();
        if (HandRenderer == null) {
            Debug.LogError("HandRenderer not found");
            Destroy(gameObject);
        }
	}

}