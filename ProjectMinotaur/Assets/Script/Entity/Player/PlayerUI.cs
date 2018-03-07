using UnityEngine;

public class PlayerUI : MonoBehaviour {

	public ProgressBar healthBar;
	public ProgressBar ammoBar;
	public WeaponSlot primarySlot;
	public WeaponSlot secondarySlot;
	public CrosshairHandler crossshair;

	private Player player;

	void Start() {
		primarySlot.SetSelected(true);
		secondarySlot.SetSelected(false);
	}

	void Update() {
		if (GetPlayer() == null || player.Toolbar == null || player.Toolbar.GetWeapon() == null || !GameStateHandler.Instance.State.Equals(GameState.INGAME)) {
			healthBar.gameObject.SetActive(false);
			ammoBar.gameObject.SetActive(false);
			primarySlot.gameObject.SetActive(false);
			secondarySlot.gameObject.SetActive(false);
			crossshair.gameObject.SetActive(false);
		} else {
			healthBar.gameObject.SetActive(true);
			ammoBar.gameObject.SetActive(player.Toolbar.GetWeapon().WeaponType.ammoPerClip > 0);
			primarySlot.gameObject.SetActive(true);
			secondarySlot.gameObject.SetActive(true);
			crossshair.gameObject.SetActive(true);

			healthBar.Progress = GetPlayer().GetHealth() / 100.0f;
			ammoBar.Progress = (float) player.Toolbar.GetWeapon().GetCurrentClipAmmo() / player.Toolbar.GetWeapon().WeaponType.ammoPerClip;
			if (ammoBar.Child != null) {
				ammoBar.Child.text = player.Toolbar.GetWeapon().GetClipCount() + " | " + player.Toolbar.GetWeapon().GetCurrentClipAmmo();
			}
		}
	}

	private Player GetPlayer() {
		if (player == null) {
			player = FindObjectOfType<Player>();
		}
		return player;
	}

}