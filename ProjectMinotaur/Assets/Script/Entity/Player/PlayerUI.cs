using UnityEngine;

public class PlayerUI : MonoBehaviour {

	public ProgressBar healthBar;

	private Player player;

	void Update() {
		if (GetPlayer() == null) {
			healthBar.gameObject.SetActive(false);
		} else {
			healthBar.gameObject.SetActive(true);

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