using UnityEngine;

public class PlayerUI : MonoBehaviour {

	public ProgressBar healthBar;
	public WeaponSlot primarySlot;
	public WeaponSlot secondarySlot;

	private Player player;

	void Start() {
		primarySlot.SetSelected(true);
		secondarySlot.SetSelected(false);
	}

	void Update() {
		if (GetPlayer() == null) {
			healthBar.gameObject.SetActive(false);
			primarySlot.gameObject.SetActive(false);
			secondarySlot.gameObject.SetActive(false);
		} else {
			healthBar.gameObject.SetActive(true);
			primarySlot.gameObject.SetActive(true);
			secondarySlot.gameObject.SetActive(true);

			healthBar.progress = GetPlayer().GetHealth() / 100.0f;
		}
	}

	private Player GetPlayer() {
		if (player == null) {
			player = FindObjectOfType<Player>();
		}
		return player;
	}

}